using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;


public class AIResponse
{
    public List<ChatMessageContent> Results { get; set; }
}
public class AIResppnseRootObject
{
    public List<object> ToolCalls { get; set; }
    public string AuthorName { get; set; }
    public Role Role { get; set; }
    public string Content { get; set; }
    public List<Item> Items { get; set; }
    public Encoding Encoding { get; set; }
    public object Source { get; set; }
    public string MimeType { get; set; }
    public InnerContent InnerContent { get; set; }
    public string ModelId { get; set; }
    public Metadata Metadata { get; set; }
}

public class Role
{
    public string Label { get; set; }
}

public class Item
{
    public string Text { get; set; }
    public Encoding Encoding { get; set; }
    public string MimeType { get; set; }
    public string InnerContent { get; set; }
    public string ModelId { get; set; }
    public Metadata Metadata { get; set; }
}

public class Encoding
{
    public string BodyName { get; set; }
    public string EncodingName { get; set; }
    public string HeaderName { get; set; }
    public string WebName { get; set; }
    public int WindowsCodePage { get; set; }
    public bool IsBrowserDisplay { get; set; }
    public bool IsBrowserSave { get; set; }
    public bool IsMailNewsDisplay { get; set; }
    public bool IsMailNewsSave { get; set; }
    public bool IsSingleByte { get; set; }
    public Fallback EncoderFallback { get; set; }
    public Fallback DecoderFallback { get; set; }
    public bool IsReadOnly { get; set; }
    public int CodePage { get; set; }
}

public class Fallback
{
    public string DefaultString { get; set; }
    public int MaxCharCount { get; set; }
}

public class InnerContent
{
    public DateTime CreatedAt { get; set; }
    public int FinishReason { get; set; }
    public List<object> ContentTokenLogProbabilities { get; set; }
    public List<object> RefusalTokenLogProbabilities { get; set; }
    public int Role { get; set; }
    public List<Content> Content { get; set; }
    public List<object> ToolCalls { get; set; }
    public object Refusal { get; set; }
    public object FunctionCall { get; set; }
    public object OutputAudio { get; set; }
    public List<object> Annotations { get; set; }
    public string Id { get; set; }
    public string Model { get; set; }
    public string SystemFingerprint { get; set; }
    public Usage Usage { get; set; }
}

public class Content
{
    public int Kind { get; set; }
    public string Text { get; set; }
    public object ImageUri { get; set; }
    public object ImageBytes { get; set; }
    public object ImageBytesMediaType { get; set; }
    public object InputAudioBytes { get; set; }
    public object InputAudioFormat { get; set; }
    public object FileId { get; set; }
    public object FileBytes { get; set; }
    public object FileBytesMediaType { get; set; }
    public object Filename { get; set; }
    public object ImageDetailLevel { get; set; }
    public object Refusal { get; set; }
}

public class Usage
{
    public int OutputTokenCount { get; set; }
    public int InputTokenCount { get; set; }
    public int TotalTokenCount { get; set; }
    public TokenDetails OutputTokenDetails { get; set; }
    public TokenDetails InputTokenDetails { get; set; }
}

public class TokenDetails
{
    public int ReasoningTokenCount { get; set; }
    public int AudioTokenCount { get; set; }
    public int AcceptedPredictionTokenCount { get; set; }
    public int RejectedPredictionTokenCount { get; set; }
    public int CachedTokenCount { get; set; }
}

public class Metadata
{
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string SystemFingerprint { get; set; }
    public Usage Usage { get; set; }
    public object Refusal { get; set; }
    public string FinishReason { get; set; }
    public List<object> ContentTokenLogProbabilities { get; set; }
}
