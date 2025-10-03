import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = 'https://your-api-url/api/insert'; // Replace with your actual API URL

  constructor(private http: HttpClient) { }

  // insertProduct(product: any): Observable<any> {
  //   const headers = new HttpHeaders({
  //     'Content-Type': 'application/json'
  //     // Add authorization header if needed
  //     // 'Authorization': `Bearer ${this.getToken()}`
  //   });

  //   //return this.http.post(`${this.apiUrl}/IN001`, product, { headers });
  //   return this.http.post(environment.apiBaseUrl + '/insert/IN001');
    
  // }

  insertProduct(formData:any){
    //return this.http.post(environment.apiBaseUrl+ '/account/signin', formData);
    return this.http.post(environment.apiBaseUrl + '/insert/IN001', formData);
  }

  // Helper method to get token if needed
  private getToken(): string {
    return localStorage.getItem('token') || '';
  }
}