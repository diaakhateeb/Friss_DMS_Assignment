import { Component, Inject, OnInit, NgModule } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserService } from "../../../Services/UserService"
import { AuthService } from "../../../Services/AuthService";
import { FormsModule, ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html'
})

@NgModule({
  imports: [FormsModule, ReactiveFormsModule],
  declarations: [UserService],
  exports: [UserService]
})

export class UserDataComponent implements OnInit {
  newUserForm: FormGroup;
  submitted = false;
  users: any[];
  userServiceObject: UserService;
  regUser: AuthService;
  loggerName: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private formBuilder: FormBuilder) {
    this.userServiceObject = new UserService(http, baseUrl);
    this.regUser = new AuthService(http, baseUrl);
  }

  ngOnInit(): void {
    this.newUserForm = this.formBuilder.group({
      name: ['', Validators.required],
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.required]],
      role: ['Member', [Validators.required, Validators.required]]
    });

    this.loggerName = localStorage.getItem("fullName");
    this.userServiceObject.getAllUsers().subscribe(result => {
      this.users = result;
      console.log(this.users);
    }, error => console.error(error));
  }

  get formValues() {
    return this.newUserForm.controls;
  }

  onSubmit() {
    this.submitted = true;
    if (this.newUserForm.invalid) return;

    this.addUser();
  }

  private addUser(): void {
    this.regUser.registerUser(this.newUserForm.value.name, this.newUserForm.value.username, this.newUserForm.value.password,
      this.newUserForm.value.email, this.newUserForm.value.role).subscribe(result => {
        console.log(result);
        this.users.push({
          id: result.newUser.id,
          fullName: result.newUser.fullName,
          email: result.newUser.email
        });
      },
        error => console.error(error));
  }

  deleteUser(id: number): void {
    const confirmDelete = confirm("Are you sure?");
    if (confirmDelete) {
      this.userServiceObject.deleteUser(id).subscribe(() => {
        this.users = this.users.filter(u => u.id !== id);
      }, error => {
        console.log("Error is: " + error.error);
        alert(error.error);
      });
    }
  }
}
