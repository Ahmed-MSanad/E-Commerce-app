import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../Core/Services/auth.service';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-register.component',
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  registerForm: FormGroup;
  showPassword = false;
  isSubmitting = false;
  isSuccess = false;

  constructor(private fb: FormBuilder) {
    this.registerForm = this.fb.group({
      id: [Date.now().toString(36) + Math.random().toString(36).substring(2), [Validators.required]],
      username: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8), this.passwordValidator]]
    });
  }

  passwordValidator(control: AbstractControl): {[key: string]: any} | null {
    const value = control.value;
    if (!value) return null;
    
    const hasLowerCase = /[a-z]/.test(value);
    const hasNumeric = /\d/.test(value);
    
    if (!hasLowerCase || !hasNumeric) {
      return { 'passwordStrength': true };
    }
    return null;
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  private readonly _auth = inject(AuthService);
  onSubmit(): void {
    if (this.registerForm.valid) {
      this.isSubmitting = true;
      
      this._auth.CreateNewUser(this.registerForm.value).subscribe({
        next:(res) => {
          this.isSubmitting = false;
          this.isSuccess = true;
          console.log(res);
        },
        error:(err) => {
          console.log(err.error);
        }
      });
    } else {
      this.registerForm.markAllAsTouched();
    }
  }

  private readonly _router = inject(Router);
  resetForm(): void {
    this.isSuccess = false;
    this.registerForm.reset();
    this.showPassword = false;
    this._router.navigate(["/Login"]);
  }

  // Helper methods for template
  hasError(fieldName: string, errorType?: string): boolean {
    const field = this.registerForm.get(fieldName);
    if (errorType) {
      return !!(field?.hasError(errorType) && field?.touched);
    }
    return !!(field?.invalid && field?.touched);
  }

  getErrorMessage(fieldName: string): string {
    const field = this.registerForm.get(fieldName);
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
