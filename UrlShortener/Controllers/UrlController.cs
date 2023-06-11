using UrlShortener.Models;
using UrlShortener.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Net;
using System.Text;
using UrlShortener.Helper;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using System;
using static System.Net.WebRequestMethods;

namespace UrlShortener.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrlController : ControllerBase
{
    private readonly UrlService _urlService;
    private readonly string defaultUrl = "http://short.cut/";
    private readonly UrlShortenerHelper _urlShortenerHelper;

    public UrlController(UrlService urlService,UrlShortenerHelper urlShortenerHelper)
    {
        _urlService = urlService;
        _urlShortenerHelper = urlShortenerHelper;
    }

    [HttpGet("~/GetAllUrls")]
    public async Task<List<Url>> Get() =>
    await _urlService.GetAsync();


    [HttpPost("~/ShortenedUrl")]
    public async Task<IActionResult> ShortenedUrl(string realUrl)
    {
        try
        {
            var isUrlPersist = await _urlService.CheckUrlAsync(realUrl);
            if (isUrlPersist)
            {
                Url persistedUrl = await _urlService.GetByRealUrlAsync(realUrl);
                return CreatedAtAction(nameof(ShortenedUrl), persistedUrl.ShortUrl);
            }
            if (!_urlShortenerHelper.IsValidUrl(realUrl))
            {
                throw new ArgumentException("Invalid URL format.", nameof(realUrl));
            }

            var hash = _urlShortenerHelper.GenerateHash(realUrl);
            var shortUrl = $"{defaultUrl}{hash}";
            Url url = new Url { Hash = hash, RealUrl = realUrl, ShortUrl = shortUrl };
            await _urlService.CreateAsync(url);
            return CreatedAtAction(nameof(ShortenedUrl), url.ShortUrl);
        }
        catch (Exception ex)
        {
            throw new Exception("Error occurred while shortening URL", ex);
        }
    }


    [HttpPost("~/CustomShortenedUrl")]
    public async Task<IActionResult> CustomShortenedUrl(string realUrl, string shortUrl)
    {
        try
        {
            var isUrlPersist = await _urlService.CheckUrlAsync(realUrl);
            if (isUrlPersist)
            {
                Url persistedUrl = await _urlService.GetByRealUrlAsync(realUrl);
                persistedUrl.ShortUrl = shortUrl;
                await _urlService.UpdateByRealUrlAsync(persistedUrl.Hash, persistedUrl);
                return CreatedAtAction(nameof(CustomShortenedUrl), persistedUrl.ShortUrl);

            }
            if (!_urlShortenerHelper.IsValidUrl(realUrl))
            {
                throw new ArgumentException("Invalid URL format.", nameof(realUrl));
            }

            var hash = _urlShortenerHelper.GenerateHash(realUrl);
            Url url = new Url { Hash = hash, RealUrl = realUrl, ShortUrl = shortUrl };
            await _urlService.CreateAsync(url);
            return CreatedAtAction(nameof(CustomShortenedUrl), url.ShortUrl);
        }
        catch (Exception ex)
        {
            throw new Exception("Error occurred while shortening URL", ex);
        }
    }

    [HttpGet("~/RedirectToUrl")]
    public async Task<IActionResult> RedirectToUrl(string shortUrl)
    {
        var url = await _urlService.GetByShortUrlAsync(WebUtility.UrlDecode(shortUrl));
        if (url != null)
        {
            return Redirect(url.RealUrl);
        }

        return NotFound("URL not found");
    }

}