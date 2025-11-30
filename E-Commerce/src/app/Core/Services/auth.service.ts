import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private _httpClient : HttpClient) { }

  CreateNewUser(userData : any){
    return this._httpClient.post(`${environment.apiBaseUrl}/users`, userData);
  }

  Login(UserData : any){
    return this._httpClient.post(`${environment.apiBaseUrl}/auth/login`, UserData);
  }
}
