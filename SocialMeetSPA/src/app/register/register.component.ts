import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';

import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { BsDatepickerConfig } from 'ngx-bootstrap';

import { User } from '../_models/user';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegister = new EventEmitter();

  bsConfig:Partial<BsDatepickerConfig>;
  registerForm:FormGroup;
  user:User;

  constructor(
    private authService:AuthService, 
    private alertify:AlertifyService,
    private fb:FormBuilder,
    private router:Router
  ) { }

  ngOnInit() {

    this.bsConfig = {
      containerClass: 'theme-red'
    }
    
    // this.registerForm = new FormGroup({
    //   username: new FormControl('', Validators.required),
    //   password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
    //   confirmPassword: new FormControl('', Validators.required)
    // }, this.passwordMatchValidator);
    // same as
    this.createRegisterForm();

  }

  createRegisterForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', Validators.required]
    }, { validator: this.passwordMatchValidator })
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value ? null : { 'mismatch': true}
  }

  register() {

    if (this.registerForm.valid) {
      this.user = Object.assign({}, this.registerForm.value);
      this.authService.register(this.user)
        .subscribe(
          () => {
            this.alertify.success('Registration successful');
          },
          error => {
            this.alertify.error(error);
          },
          () => {
            this.authService.login(this.user)
              .subscribe(
                () => {
                  this.router.navigate(['/members'])
                }
              )
          }
        )
    }

  }

  cancel() {
    this.cancelRegister.emit(false);
  }

}
