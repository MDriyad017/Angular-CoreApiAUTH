import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TOKEN_KEY } from '../constant';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http:HttpClient) { }
  baseUrl = 'http://localhost:46126/api';

  createUser(formData:any){
    return this.http.post(this.baseUrl + '/account/signup', formData);
  }

  signin(formData:any){
    return this.http.post(this.baseUrl + '/account/signin', formData);
  }

  isLoggedIn(){
    return localStorage.getItem(TOKEN_KEY) != null ? true : false;
  }

  saveToken(token:string){
    localStorage.setItem(TOKEN_KEY, token);
  }

  deleteToken(){
    localStorage.removeItem(TOKEN_KEY);
  }

}
