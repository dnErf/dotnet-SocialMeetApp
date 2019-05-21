import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router/src/utils/preactivation';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(
    private authService:AuthService,
    private router:Router,
    private alertify:AlertifyService
  ) {}

  canActivate():boolean {
    if (this.authService.loggedIn()) {
      return true;
    }
    this.alertify.error('cannot navigate to the desired path');
    this.router.navigate(['/home']);
    return false;
  }

}
