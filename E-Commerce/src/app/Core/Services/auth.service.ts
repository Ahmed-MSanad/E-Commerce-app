import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { FormGroup } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private _httpClient : HttpClient) { }

  Register(userData : any){
    return this._httpClient.post(`${environment.apiBaseUrl}/Authentication/Register`, userData);
  }

  Login(userData : any){
    return this._httpClient.post(`${environment.apiBaseUrl}/Authentication/Login`, userData);
  }

  hasError(fieldName: string, from:FormGroup, errorType?: string): boolean {
    const field = from.get(fieldName);
    if (errorType) {
      return !!(field?.hasError(errorType) && field?.touched);
    }
    return !!(field?.invalid && field?.touched);
  }

  getErrorMessage(fieldName: string, from:FormGroup): string {
    const field = from.get(fieldName);
    if (field?.hasError('required')) {
      return `${fieldName.charAt(0).toUpperCase() + fieldName.slice(1)} is required`;
    }
    if (field?.hasError('minlength')) {
      const requiredLength = field.errors?.['minlength'].requiredLength;
      return `${fieldName.charAt(0).toUpperCase() + fieldName.slice(1)} must be at least ${requiredLength} characters`;
    }
    if (field?.hasError('email')) {
      return 'Please enter a valid email address';
    }
    if (field?.hasError('passwordStrength')) {
      return 'Password must contain uppercase, lowercase, and number';
    }
    return '';
  }
}
