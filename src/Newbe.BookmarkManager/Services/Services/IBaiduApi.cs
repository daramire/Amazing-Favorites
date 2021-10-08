﻿
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Refit;

namespace Newbe.BookmarkManager.Services
{
    public interface IBaiduApi
    {
        [Get("/api/quota")]
        Task<ApiResponse<BaiduQuotaResponse>> GetQuotaAsync(BaiduQuotaRequest baiduQuotaRequest);

        [Get("/rest/2.0/xpan/file?method=list")]
        Task<ApiResponse<BaiduFileListResponse>> GetFileListAsync(BaiduFileListRequest baiduFileListRequest);

        [Get("/rest/2.0/xpan/file?method=doclist")]
        Task<ApiResponse<BaiduDocListResponse>> GetDocListAsync(BaiduDocListRequest baiduDocListRequest);
        
        [Get("/rest/2.0/xpan/file?method=search")]
        Task<ApiResponse<BaiduSearchResponse>> SearchAsync(BaiduSearchRequest baiduSearchRequest);

        [Get("/rest/2.0/xpan/multimedia?method=filemetas")]
        Task<ApiResponse<BaiduFileMetasResponse>> GetFileMatesAsync(BaiduFileMetasRequest baiduFileMetasRequest);
    }

    public record BaiduQuotaRequest
    {
        [AliasAs("access_token")]
        public string AccessToken { get; set; }
        [AliasAs("checkfree")]
        public int CheckFree { get; set; }
        [AliasAs("checkexpire")]
        public int CheckExpire { get; set; }
    }
    public record BaiduQuotaResponse
    {
        [JsonPropertyName("errno")]
        public long Errno { get; set; }
        [JsonPropertyName("total")]
        public long Total { get; set; }
        [JsonPropertyName("expire")]
        public bool Expire { get; set; }
        [JsonPropertyName("used")]
        public long Used { get; set; }
        [JsonPropertyName("free")]
        public long Free { get; set; }
        [JsonPropertyName("request_id")]
        public long RequestId { get; set; }
    }

    public record BaiduFileListRequest
    {
        [AliasAs("access_token")]
        public string AccessToken { get; set; }
        [AliasAs("dir")]
        public string? Dir { get; set; }
        [AliasAs("order")]
        public string? Order { get; set; }
        [AliasAs("desc")]
        public string? Desc { get; set; }
        [AliasAs("start")]
        public int? Start { get; set; }
        [AliasAs("limit")]
        public int? Limit { get; set; }
        [AliasAs("web")]
        public string? Web { get; set; }
        [AliasAs("folder")]
        public int? Folder { get; set; }
        [AliasAs("showempty")]
        public int? ShowEmpty { get; set; }
    }

    public record BaiduFileListResponse
    {
        [JsonPropertyName("errno")]
        public string Errno { get; set; }
        [JsonPropertyName("guid")]
        public long Guid { get; set; }
        [JsonPropertyName("guid_info")]
        public string GuidInfo { get; set; }
        [JsonPropertyName("request_id")]
        public long RequestId { get; set; }
        [JsonPropertyName("list")]
        public BaiduFile[] List { get; set; }
    }

    public record BaiduFile
    {
        [JsonPropertyName("category")]
        public int Category { get; set; }
        [JsonPropertyName("dir_empty")]
        public int DirEmpty { get; set; }
        [JsonPropertyName("empty")]
        public int Empty { get; set; }
        [JsonPropertyName("extent_tinyint7")]
        public int ExtentTinyInt7 { get; set; }
        [JsonPropertyName("fs_id")]
        public long FsId { get; set; }
        [JsonPropertyName("isdir")]
        public int IsDir { get; set; }
        [JsonPropertyName("local_mtime")]
        public long LocalMTime { get; set; }
        [JsonPropertyName("local_ctime")]
        public long LocalCTime { get; set; }
        [JsonPropertyName("oper_id")]
        public long OperId { get; set; }
        [JsonPropertyName("owner_id")]
        public int OwnerId { get; set; }
        [JsonPropertyName("owner_type")]
        public int OwnerType { get; set; }
        [JsonPropertyName("path")]
        public string Path { get; set; }
        [JsonPropertyName("pl")]
        public int PL { get; set; }
        [JsonPropertyName("real_category")]
        public string realCategory { get; set; }
        [JsonPropertyName("server_atime")]
        public long ServerATime { get; set; }
        [JsonPropertyName("server_ctime")]
        public long ServerCTime { get; set; }
        
        [JsonPropertyName("server_filename")]
        public string ServerFileName { get; set; }
        [JsonPropertyName("server_mtime")]
        public long ServerMTime { get; set; }
        [JsonPropertyName("share")]
        public int Share { get; set; }
        [JsonPropertyName("size")]
        public int Size { get; set; }
        [JsonPropertyName("tkbind_id")]
        public long TKBindId { get; set; }
        [JsonPropertyName("unlist")]
        public int UnList { get; set; }
        [JsonPropertyName("wpfile")]
        public int WPFile { get; set; }
        
        [JsonPropertyName("md5")]
        public string MD5 { get; set; }
    }

    public record BaiduDocListRequest
    {
        [AliasAs("access_token")]
        public string AccessToken { get; set; }
        [AliasAs("page")]
        public int? Page { get; set; }
        [AliasAs("num")]
        public int? Num { get; set; }
        [AliasAs("order")]
        public string? Order { get; set; }
        [AliasAs("desc")]
        public string? Desc { get; set; }
        [AliasAs("parent_path")]
        public string? ParentPath { get; set; }
        [AliasAs("recursion")]
        public string? Recursion { get; set; }
        [AliasAs("web")]
        public int? Web { get; set; }
    }
    public record BaiduDocListResponse
    {
        
    }

    public record BaiduSearchRequest
    {
        [AliasAs("access_token")]
        public string AccessToken { get; set; }
        [AliasAs("key")]
        public string Key { get; set; }
        [AliasAs("dir")]
        public string Dir { get; set; }
        [AliasAs("page")]
        public int? Page { get; set; }
        [AliasAs("num")]
        public int? Num { get; set; }
        [AliasAs("recursion")]
        public string? Recursion { get; set; }
        [AliasAs("web")]
        public int? Web { get; set; }
    }
    public record BaiduSearchResponse
    {
        public dynamic ContentList { get; set; }
        [JsonPropertyName("errno")]
        public long Errno { get; set; }
        [JsonPropertyName("has_more")]
        public int HasMore { get; set; }
        [JsonPropertyName("request_id")]
        public long RequestId { get; set; }
        [JsonPropertyName("list")]
        public BaiduFile[] List { get; set; }
    }

    public record BaiduFileMetasRequest
    {
        [AliasAs("access_token")]
        public string AccessToken { get; set; }
        
        [AliasAs("fsids")]
        public string FsIds { get; set; }
        
        [AliasAs("path")]
        public string? Path { get; set; }
        [AliasAs("thumb")]
        public int? Thumb { get; set; }
        [AliasAs("dlink")]
        public int? DLink { get; set; }
        [AliasAs("extra")]
        public int? Extra { get; set; }
    }

    public record BaiduFileMetasResponse
    {
        [JsonPropertyName("errmsg")]
        public string ErrMsg { get; set; }
        [JsonPropertyName("errno")]
        public long Errno { get; set; }
        
        //[JsonPropertyName("names")]
        //public string[] Names { get; set; }

        [JsonPropertyName("list")]
        public BaiduFileDLink[] List { get; set; }
        [JsonPropertyName("request_id")]
        public string RequestId { get; set; }
    }

    public record BaiduFileDLink
    {
        [JsonPropertyName("category")]
        public int Category { get; set; }
        [JsonPropertyName("dlink")]
        public string DLink { get; set; }
        [JsonPropertyName("filename")]
        public string FileName { get; set; }
        [JsonPropertyName("fs_id")]
        public long FsId { get; set; }
        [JsonPropertyName("isdir")]
        public int IsDir { get; set; }
        [JsonPropertyName("md5")]
        public string MD5 { get; set; }

        [JsonPropertyName("oper_id")]
        public long OperId { get; set; }
        [JsonPropertyName("path")]
        public string Path { get; set; }
        
        [JsonPropertyName("server_ctime")]
        public long ServerCTime { get; set; }
        [JsonPropertyName("server_mtime")]
        public long ServerMTime { get; set; }
        [JsonPropertyName("size")]
        public long Size { get; set; }
    }
    
}