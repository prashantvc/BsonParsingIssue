using Metsys.Bson;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

Stream _inputStream = new MemoryStream();
var _resolver = new DefaultMessageTypeResolver();

// byte array from js bson message for object { "dpiX" : 192, "dpiY" : 192 }
// using BSON.serialize https://www.npmjs.com/package/bson
var data = new byte[]
{ 25, 0, 0, 0, 211, 37, 60, 122, 82, 54, 141, 67, 142, 241, 134, 233, 66, 204, 150, 192, 25, 0, 0, 0, 16, 100, 112, 105, 88, 0, 192, 0, 0, 0, 16, 100, 112, 105, 89, 0, 192, 0, 0, 0, 0 };

var message = data.Skip(20).Take(data.Length).ToArray();
// desierialize message perfectly to bson document
PrintBufferBsonReader(message);


//fails if use Metsys.Bson Deserializer
await _inputStream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
_inputStream.Seek(0, SeekOrigin.Begin);
await Reader().ConfigureAwait(false);


async Task Reader()
{
    //read header
    var infoBlock = new byte[20];
    await ReadExact(infoBlock).ConfigureAwait(false);
    var length = BitConverter.ToInt32(infoBlock, 0);
    var guidBytes = new byte[16];
    Buffer.BlockCopy(infoBlock, 4, guidBytes, 0, 16);
    var guid = new Guid(guidBytes);

    //read message
    var buffer = new byte[length];
    await ReadExact(buffer).ConfigureAwait(false);

    var messageStream = new MemoryStream(buffer);
    var msgBR = new BinaryReader(messageStream);
    Type t = _resolver.GetByGuid(guid);

    // EndOfStreamException exception here
    var message = Deserializer.Deserialize(msgBR, t);
    Console.WriteLine(message);
}

async Task ReadExact(byte[] buffer)
{
    int read = 0;
    while (read != buffer.Length)
    {
        var readNow = await _inputStream.ReadAsync(buffer, read, buffer.Length - read, new CancellationToken())
            .ConfigureAwait(false);
        if (readNow == 0)
            throw new EndOfStreamException();
        read += readNow;
    }
}

void PrintBufferBsonReader(byte[] v)
{
    //convert buffer to string
    using var ms = new MemoryStream(v);
    using var reader = new BsonBinaryReader(ms);
    var bson = BsonSerializer.Deserialize<BsonDocument>(reader);
    Console.WriteLine(bson);

}