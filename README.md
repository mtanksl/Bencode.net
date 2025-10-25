# B-encode.net

An implementation in C# of B-encode format that is used by the peer-to-peer file sharing system BitTorrent.

# Install using NuGet
![Nuget](https://img.shields.io/nuget/v/mtanksl.Bencode)

```
dotnet add package mtanksl.Bencode --version 1.1.0
```

# Specification

From [Wikipedia](https://en.wikipedia.org/wiki/Bencode):

- Byte Strings are encoded as `<length>:<bytes>`
- Integers are encoded as `i<integer>e`
- Lists are encoded as `l<elements>e`
- Dictionaries are encoded as `d<pairs>e`

# Creating with LINQ

```C#
BElement value = 
    new BDictionary(
        new BPair("key 1", ""),
        new BPair("key 3", 9223372036854775807L),
        new BPair("key 2", "Hello World"),
        new BPair("key 4", new BList("", "Hello World", 9223372036854775807L) ) );
```

# How to serialize an object

```C#
string bencode = BencodeConvert.SerializeObject(value);

// "d5:key 10:5:key 211:Hello World5:key 3i9223372036854775807e5:key 4l0:11:Hello Worldi9223372036854775807eee"
```

or

```C#
using (var stream = File.OpenWrite("...") )
{
    using (var writer = new BencodeWriter(stream) )
    {
        var serializer = new BencodeSerializer();

        serializer.Serialize(writer, value);
    }
}
```

# How to deserialize an object

```C#
BElement value = (BElement)BencodeConvert.DeserializeObject(bencode);
```

or

```C#
using (var stream = File.OpenRead("...") )
{
    using (var reader = new BencodeReader(stream) )
    {
        var serializer = new BencodeSerializer();

        var value = (BElement)serializer.Deserialize(reader);

        //...
    }
}
```


# Querying with LINQ

```C#
string helloWorld = (string)value["key 4"][1];

// "Hello World"
```

*(See the Tests project for more examples)*
