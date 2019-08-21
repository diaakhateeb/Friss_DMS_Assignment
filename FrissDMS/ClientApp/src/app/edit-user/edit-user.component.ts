import { Component, Inject, OnInit, NgModule } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserService } from "../../../Services/UserService"
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-user',
  templateUrl: './edit-user.component.html'
})
@NgModule({
  declarations: [UserService],
  exports: [UserService]
})
export class EditUserComponent implements OnInit {
  userServiceObject: UserService;
  id: number;
  name: string;
  email: string;
  username: string;
  password: string;
  role: string;
  loggerName: string;

  constructor(http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    private route: ActivatedRoute,
    private readonly router: Router) {
    this.userServiceObject = new UserService(http, baseUrl);
    this.id = this.route.snapshot.queryParams["id"];
  }

  ngOnInit(): void {
    this.loggerName = localStorage.getItem("fullName");
    this.userServiceObject.getUser(this.id).subscribe(result => {
      this.name = result.user.fullName;
      this.email = result.user.email;
      this.username = result.user.userName;
      //this.password = result.password;
      this.role = result.role;
    },
      error => console.error(error));
  }

  setRole(role: string) {
    this.role = role;
  }

  editUser(): void {
    this.userServiceObject.editUser(this.id, this.name, this.username, this.email, this.password, this.role).subscribe(result => {
      this.router.navigate(["/user"]);
    }, error => console.error(error));
  }
}
