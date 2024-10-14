import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AppComponent } from './app.component';
import { UrlShortnerService, UrlShortener } from './Service/url-shortner.service';
import { of, throwError } from 'rxjs';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let urlShortenerService: jasmine.SpyObj<UrlShortnerService>;
  let mockUrlShorteners: UrlShortener[];

  beforeEach(() => {
    const urlShortenerServiceSpy = jasmine.createSpyObj('UrlShortnerService', ['getShortenedUrls', 'shortenUrl']);

    TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, HttpClientTestingModule],
      declarations: [AppComponent],
      providers: [{ provide: UrlShortnerService, useValue: urlShortenerServiceSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    urlShortenerService = TestBed.inject(UrlShortnerService) as jasmine.SpyObj<UrlShortnerService>;

    // Mock data for shortened URLs
    mockUrlShorteners = [
      { urlShortenerId: 1, originalUrl: 'https://example.com', shortUrl: 'https://short.ly/abc123' },
      { urlShortenerId: 2, originalUrl: 'https://another.com', shortUrl: 'https://short.ly/xyz456' }
    ];
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch shortened URLs on init', () => {
    // Arrange
    urlShortenerService.getShortenedUrls.and.returnValue(of(mockUrlShorteners));

    // Act
    component.ngOnInit();
    fixture.detectChanges();

    // Assert
    expect(urlShortenerService.getShortenedUrls).toHaveBeenCalled();
    expect(component.shortenedUrls.length).toBe(2);
    expect(component.shortenedUrls).toEqual(mockUrlShorteners);
  });

  it('should handle error when fetching URLs fails', () => {
    // Arrange
    urlShortenerService.getShortenedUrls.and.returnValue(throwError('Failed to fetch data'));

    // Act
    component.ngOnInit();
    fixture.detectChanges();

    // Assert
    expect(component.errorMessage).toBe('Failed to fetch data');
  });

  it('should submit a valid URL and fetch new shortened URLs', () => {
    // Arrange
    const newUrl = 'https://newurl.com';
    const shortenedUrl: UrlShortener = { urlShortenerId: 3, originalUrl: newUrl, shortUrl: 'https://short.ly/new123' };
    
    urlShortenerService.shortenUrl.and.returnValue(of(shortenedUrl));
    urlShortenerService.getShortenedUrls.and.returnValue(of([...mockUrlShorteners, shortenedUrl]));

    component.urlForm.controls['originalUrl'].setValue(newUrl);

    // Act
    component.onSubmit();
    fixture.detectChanges();

    // Assert
    expect(urlShortenerService.shortenUrl).toHaveBeenCalledWith(newUrl);
    expect(component.responseUrl).toEqual(shortenedUrl);
    expect(component.shortenedUrls.length).toBe(3); // Check that the new URL was added
  });

  it('should handle error when submitting an invalid URL', () => {
    // Arrange
    component.urlForm.controls['originalUrl'].setValue(''); // Set invalid URL

    // Act
    component.onSubmit();
    
    // Assert
    expect(component.errorMessage).toBe('Error shortening URL. Please try again.');
    expect(component.responseUrl).toBeNull();
  });
});
