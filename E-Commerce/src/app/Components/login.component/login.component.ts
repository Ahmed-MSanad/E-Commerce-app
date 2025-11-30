import { Component, inject } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../Core/Services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login.component',
  imports: [RouterLink, ReactiveFormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginForm: FormGroup;
  showPassword = false;
  isSubmitting = false;
  isSuccess = false;
  private readonly _auth = inject(AuthService);
  private readonly _router = inject(Router);

  constructor(private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      username: ['mor_2314', [Validators.required]],
      password: ['83r5^_', [Validators.required]]
    });
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      this.isSubmitting = true;
      this._auth.Login(this.loginForm.value).subscribe({
        next:(res : any) => {
          this.isSubmitting = false;
          this.isSuccess = true;
          console.log(res);
          this._router.navigate(["/Products"]);
          localStorage.setItem("token", res.token);
        },
        error:(err) => {
          console.log(err.error);
        }
      });
    } else {
      this.loginForm.markAllAsTouched();
    }
  }

  resetForm(): void {
    this.isSuccess = false;
    this.loginForm.reset();
    this.showPassword = false;
    this._router.navigate(["/Login"]);
  }

  // Helper methods for template
  hasError(fieldName: string, errorType?: string): boolean {
    const field = this.loginForm.get(fieldName);
    if (errorType) {
      return !!(field?.hasError(errorType) && field?.touched);
    }
    return !!(field?.invalid && field?.touched);
  }

  getErrorMessage(fieldName: string): string {
    const field = this.loginForm.get(fieldName);
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
