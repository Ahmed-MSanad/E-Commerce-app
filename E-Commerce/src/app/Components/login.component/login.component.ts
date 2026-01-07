import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../Core/Services/auth.service';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';

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
  readonly _auth = inject(AuthService);
  private readonly _router = inject(Router);

  constructor(private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
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
          Swal.fire({
            icon: 'success',
            title: 'Login Successful',
            text: 'You have been logged in successfully!',
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
      this.loginForm.markAllAsTouched();
    }
  }
}
