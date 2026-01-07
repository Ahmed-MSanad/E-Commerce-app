import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../Core/Services/auth.service';
import { Router, RouterLink } from '@angular/router';
import Swal from 'sweetalert2';

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
  readonly _auth = inject(AuthService);
  private readonly _router = inject(Router);

  constructor(private fb: FormBuilder) {
    this.registerForm = this.fb.group({
      displayName: ['', [Validators.required, Validators.minLength(5)]],
      userName: ['', [Validators.required, Validators.minLength(5)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8), this.passwordValidator]],
      phoneNumber: ['', [Validators.required, Validators.minLength(10)]],
    });
  }

  passwordValidator(control: AbstractControl): {[key: string]: any} | null {
    const value = control.value;
    if (!value) return null;
    
    const hasNonAlphanumeric = /[^a-zA-Z_]/.test(value);
    const hasNumeric = /\d/.test(value);

    if (!hasNonAlphanumeric || !hasNumeric) {
      return { 'passwordStrength': true };
    }
    return null;
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      this.isSubmitting = true;
      this._auth.Register(this.registerForm.value).subscribe({
        next:(res : any) => {
          Swal.fire({
            icon: 'success',
            title: 'Register Successful',
            text: 'You have been registered in successfully!',
            timer: 2000,
            showConfirmButton: false
          });
          this.isSubmitting = false;
          this.isSuccess = true;
          localStorage.setItem('token', res.token);
          localStorage.setItem('displayName', res.displayName);
          localStorage.setItem('email', res.email);
          this._router.navigate(["/Products"]);
        },
        error:(err) => {
          console.log(err.error);
        }
      });
    } else {
      this.registerForm.markAllAsTouched();
    }
  }
}
