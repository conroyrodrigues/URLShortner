import { Component, OnInit } from '@angular/core';
import { UrlShortnerService, UrlShortener } from './Service/url-shortner.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  urlForm: FormGroup;
  responseUrl: UrlShortener | null = null;
  shortenedUrls: UrlShortener[] = [];
  errorMessage: string = '';

  constructor(private urlShortenerService: UrlShortnerService, private fb: FormBuilder) 
  {
    this.urlForm = this.fb.group(
      {
        originalUrl: ['', [Validators.required, Validators.pattern('https?://.+')]]
      });
   }

  ngOnInit(): void {
    this.fetchUrls();
  }
  
  fetchUrls(): void {
    this.urlShortenerService.getShortenedUrls().subscribe(
      (urls) => this.shortenedUrls = urls,
      (error) => this.errorMessage = 'Failed to fetch data'
    );
  }
  
  onSubmit(): void {
    if (this.urlForm.valid) {
      this.urlShortenerService.shortenUrl(this.urlForm.value.originalUrl).subscribe({
        next: (response) => {
          // Store the response
          this.responseUrl = response; 
          // Clear any previous error messages
          this.errorMessage = ''; 
          //Refresh the Table
          this.fetchUrls();
        },
        error: (err) => {
          this.errorMessage = err.error || 'Error shortening URL. Please try again.';
          // Clear any previous responses
          this.responseUrl = null; 
        }
      });
    }else{
      this.errorMessage = 'Error shortening URL. Please try again.'; 
      this.responseUrl = null; 
    }
}
}