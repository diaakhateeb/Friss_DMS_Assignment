import { Component, Inject, NgModule, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../../../Services/AuthService';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html'
})

@NgModule({
  imports: [FormsModule, ReactiveFormsModule, FormGroup],
  declarations: [AuthService],
  exports: [AuthService]
})

export class RegistrationComponent implements OnInit {
  ngOnInit(): void {
    this.registrationForm = this.formBuilder.group({
      fullName: ['', Validators.required],
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.required]],
      confirmPassword: ['', [Validators.required]]
    });
  }

  registrationForm: FormGroup;
  submitted = false;
  authService: AuthService;
  router: Router;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private formBuilder: FormBuilder, router: Router) {
    this.authService = new AuthService(http, baseUrl);
    this.router = router;
  }

  registerUser(): void {
    if (this.checkBothPasswordMatch()) {
      console.log(this.registrationForm.value.email);
      this.authService.registerUser(
        this.registrationForm.value.fullName, this.registrationForm.value.username,
        this.registrationForm.value.password, this.registrationForm.value.email, "Member").subscribe(
          result => {
            this.router.navigate(["login"]);
          },
          error => console.error(error));
    } else alert("Password does not match.");
  }

  get formValues() {
    return this.registrationForm.controls;
  }

  onSubmit() {
    this.submitted = true;
    if (this.registrationForm.invalid) return;

    this.registerUser();
  }


  checkBothPasswordMatch(): boolean {
    if (this.registrationForm.value.password !== this.registrationForm.value.confirmPassword) {
      return false;
    }
    return true;
  }
}
