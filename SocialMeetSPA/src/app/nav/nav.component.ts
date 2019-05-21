import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model:any = {};
  uniqueName:string = '';

  constructor(private authService:AuthService, private alertify:AlertifyService) { }

  ngOnInit() {
    this.uniqueName = this.authService.decodedToken?.unique_name;
  }

  login() {
    this.authService.login(this.model)
      .subscribe(
        next => {
          this.alertify.success('Logged in successfully');
        },
        error => {
          this.alertify.error('Failed to login');
        }
      );
  }

  loggedIn() {
    // const token = localStorage.getItem('token');
    // return !!token; // equivalent to if token
    return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
    this.alertify.message('logged out');
  }

}
