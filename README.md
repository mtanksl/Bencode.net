# B-encode

An implementation in C# of B-encode format that is used by the peer-to-peer file sharing system BitTorrent.

# Install using NuGet
![Nuget](https://img.shields.io/nuget/v/mtanksl.Bencode)

```
dotnet add package mtanksl.Bencode --version 1.0.0
```

# Specification

From [Wikipedia](https://en.wikipedia.org/wiki/Bencode):

- Strings are encoded as `<length>:<string>`
- Integers are encoded as `i<integer>e`
- Lists are encoded as `l<elements>e`
- Dictionaries are encoded as `d<pairs>e`

# How to serialize an object

```C#
string bencode = BencodeConvert.SerializeObject(value)
```

# How to deserialize an object

```C#
object value = BencodeConvert.DeserializeObject(bencode);
```