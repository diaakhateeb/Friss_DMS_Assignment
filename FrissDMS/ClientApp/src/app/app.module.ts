import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { DocumentComponent } from './document/documentComponent';
import { ListDocumentComponent } from './list-document/list-documentComponent';
import { RegistrationComponent } from './registration/registration.component';
import { UserDataComponent } from './user/user.component';
import { EditUserComponent } from './edit-user/edit-user.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { AccessDeniedComponent } from './access-denied/access-denied.component';
import { AuthGuardService } from '../../Services/AuthGuardService';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    DocumentComponent,
    ListDocumentComponent,
    RegistrationComponent,
    UserDataComponent,
    EditUserComponent,
    LoginComponent,
    LogoutComponent,
    AccessDeniedComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      {
        path: '',
        redirectTo: "/login",
        pathMatch: 'full'
      },
      { path: 'document', component: DocumentComponent, canActivate: [AuthGuardService], data: { roles: ['Admin'] } },
      { path: 'list-document', component: ListDocumentComponent, canActivate: [AuthGuardService], data: { roles: ['Admin', 'Member'] } },
      { path: 'registration', component: RegistrationComponent },
      { path: 'user', component: UserDataComponent, canActivate: [AuthGuardService], data: { roles: ['Admin'] } },
      { path: 'edit-user', component: EditUserComponent },
      { path: 'login', component: LoginComponent },
      { path: 'logout', component: LogoutComponent },
      { path: 'access-denied', component: AccessDeniedComponent }
    ])
  ],
  providers: [CookieService, AuthGuardService],
  bootstrap: [AppComponent]
})
export class AppModule { }
