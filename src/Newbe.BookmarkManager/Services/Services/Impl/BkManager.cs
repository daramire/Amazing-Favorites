﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newbe.BookmarkManager.WebApi;
using WebExtension.Net.Bookmarks;

namespace Newbe.BookmarkManager.Services
{
    public class BkManager : IBkManager
    {
        private readonly IBkDataHolder _bkDataHolder;
        private readonly IClock _clock;
        private readonly ILogger<BkManager> _logger;

        public BkManager(
            ILogger<BkManager> logger,
            IBkDataHolder bkDataHolder,
            IClock clock)
        {
            _bkDataHolder = bkDataHolder;
            _clock = clock;
            _logger = logger;
        }

        public async ValueTask RemoveTagAsync(string url, string tag)
        {
            var bkEntityCollection = _bkDataHolder.Collection;
            if (bkEntityCollection.Bks.TryGetValue(url, out var bk))
            {
                if (bk.Tags?.Contains(tag) == true)
                {
                    await _bkDataHolder.PushDataChangeActionAsync(() =>
                    {
                        bk.Tags.Remove(tag);
                        _logger.LogInformation("Tag {Tag} removed from {Url}", tag, url);
                    });
                }
            }
        }

        private void AddTagToCollection(string tag)
        {
            if (!_bkDataHolder.Collection.Tags.TryGetValue(tag, out var oldTag))
            {
                _logger.LogInformation("A new tag {Tag} added to all collection", oldTag);
                oldTag = new BkTag
                {
                    Tag = tag,
                    TagAlias = new()
                };
                _bkDataHolder.Collection.Tags[tag] = oldTag;
            }
        }

        public async ValueTask<bool> AddTagAsync(string url, string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                return false;
            }

            _logger.LogInformation("New tag {Tag} add to {Url}", tag, url);
            var bkEntityCollection = _bkDataHolder.Collection;
            if (bkEntityCollection.Bks.TryGetValue(url, out var bk))
            {
                var key = tag.Trim();
                if (bk.Tags != null && bk.Tags.Contains(key))
                {
                    return false;
                }

                await _bkDataHolder.PushDataChangeActionAsync(() =>
                {
                    AddTagToCollection(key);
                    bk.Tags ??= new();
                    bk.Tags.Add(key);
                    _logger.LogInformation("Tag {Tag} added for {Url}", key, url);
                });
            }

            return true;
        }

        public async ValueTask UpdateTagsAsync(string url, IEnumerable<string> tags)
        {
            var bkEntityCollection = _bkDataHolder.Collection;
            if (bkEntityCollection.Bks.TryGetValue(url, out var bk))
            {
                var tagList = tags.ToList();
                foreach (var tag in tagList)
                {
                    AddTagToCollection(tag);
                }

                bk.Tags = tagList;

                _logger.LogInformation("Tag {Tags} added for {Url}", tagList, url);
                await _bkDataHolder.SaveNowAsync();
            }
        }

        public async ValueTask UpdateFavIconUrlAsync(Dictionary<string, string> urls)
        {
            foreach (var (url, furl) in urls)
            {
                var bkEntityCollection = _bkDataHolder.Collection;
                if (bkEntityCollection.Bks.TryGetValue(url, out var bk) &&
                    bk.FavIconUrl != furl)
                {
                    await _bkDataHolder.PushDataChangeActionAsync(() =>
                    {
                        bk.FavIconUrl = furl;
                        _logger.LogInformation("FavIconUrl: {FavIconUrl} updated for url: {Url}", furl, bk.Url);
                    });
                }
            }
        }

        public async ValueTask AppendBookmarksAsync(IEnumerable<BookmarkTreeNode> nodes)
        {
            await _bkDataHolder.AppendBookmarksAsync(nodes);
        }

        public async Task LoadCloudCollectionAsync(CloudBkCollection cloudBkCollection)
        {
            var dictByUrlHash = _bkDataHolder.Collection.Bks.Values.ToDictionary(x => x.UrlHash);
            foreach (var (urlHash, cloudBk) in cloudBkCollection.Bks)
            {
                if (dictByUrlHash.TryGetValue(urlHash, out var localBk))
                {
                    await _bkDataHolder.PushDataChangeActionAsync(() => { localBk.Tags = cloudBk.Tags; });
                }
            }

            _bkDataHolder.UpdateEtagVersion(cloudBkCollection.EtagVersion);
        }

        public CloudBkCollection GetCloudBkCollection()
        {
            var local = _bkDataHolder.Collection;
            var re = new CloudBkCollection
            {
                Bks = local.Bks.Values
                    .Where(x => x.Tags.Count > 0)
                    .ToDictionary(x => x.UrlHash, x => new CloudBk
                    {
                        Tags = x.Tags,
                    }),
                LastUpdateTime = local.LastUpdateTime,
                EtagVersion = local.EtagVersion
            };
            return re;
        }

        public long GetEtagVersion()
        {
            return _bkDataHolder.Collection.EtagVersion;
        }

        public Bk? Get(string url)
        {
            var bkEntityCollection = _bkDataHolder.Collection;
            return bkEntityCollection.Bks.TryGetValue(url, out var bk) ? bk : null;
        }

        public async Task InitAsync()
        {
            await _bkDataHolder.StartAsync();
        }

        public async ValueTask AddClickAsync(string url, int moreCount)
        {
            var bkEntityCollection = _bkDataHolder.Collection;
            if (bkEntityCollection.Bks.TryGetValue(url, out var bk))
            {
                await _bkDataHolder.PushDataChangeActionAsync(() =>
                {
                    bk.ClickedCount += moreCount;
                    bk.LastClickTime = _clock.UtcNow;
                });
            }
        }

        public async ValueTask RestoreAsync()
        {
            await _bkDataHolder.RestoreAsync();
        }
    }
}