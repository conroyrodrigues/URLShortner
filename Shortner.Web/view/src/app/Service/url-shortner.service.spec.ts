import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { UrlShortnerService, UrlShortener } from './url-shortner.service';

describe('UrlShortnerService', () => {
  let service: UrlShortnerService;
  let httpMock: HttpTestingController;

  // Sample data for testing
  const mockUrlShorteners: UrlShortener[] = [
    { urlShortenerId: 1, originalUrl: 'https://example.com', shortUrl: 'https://short.ly/abc123' },
    { urlShortenerId: 2, originalUrl: 'https://test.com', shortUrl: 'https://short.ly/xyz456' }
  ];

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [UrlShortnerService]
    });

    service = TestBed.inject(UrlShortnerService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    // Verify that no unmatched requests remain
    httpMock.verify();
  });

  it('should fetch all shortened URLs from API', () => {
    service.getShortenedUrls().subscribe(urls => {
      expect(urls.length).toBe(2);
      expect(urls).toEqual(mockUrlShorteners);
    });

    const req = httpMock.expectOne(service['apiUrl']);
    expect(req.request.method).toBe('GET');
    req.flush(mockUrlShorteners); // Simulate the response
  });

  it('should post a new URL to shorten', () => {
    const originalUrl = 'https://newexample.com';
    const expectedResponse: UrlShortener = { urlShortenerId: 3, originalUrl, shortUrl: 'https://short.ly/new123' };

    service.shortenUrl(originalUrl).subscribe(response => {
      expect(response).toEqual(expectedResponse);
    });

    const req = httpMock.expectOne(service['apiUrl']);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({ originalUrl }); // Verify the request body
    req.flush(expectedResponse); // Simulate the response
  });
});
