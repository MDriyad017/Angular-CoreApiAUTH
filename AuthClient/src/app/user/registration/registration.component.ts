import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { trigger, transition, style, animate, keyframes } from '@angular/animations';
import { AuthService } from '../../shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css'],
  animations: [
    trigger('fadeInUp', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(40px)' }),
        animate('600ms ease-out', style({ opacity: 1, transform: 'translateY(0)' }))
      ])
    ]),
    trigger('shake', [
      transition(':enter', [
        animate(
          '500ms ease-in',
          keyframes([
            style({ transform: 'translateX(-10px)', offset: 0.2 }),
            style({ transform: 'translateX(10px)', offset: 0.4 }),
            style({ transform: 'translateX(-10px)', offset: 0.6 }),
            style({ transform: 'translateX(10px)', offset: 0.8 }),
            style({ transform: 'translateX(0)', offset: 1 })
          ])
        )
      ])
    ])
  ]
})
export class RegistrationComponent implements OnInit{
  constructor(
    public formBuilder: FormBuilder, 
    private service: AuthService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    if(this.service.isLoggedIn())
      this.router.navigateByUrl('/dashboard');
  }

  isSubmitted:boolean = false;

  passWordMatchValid: ValidatorFn = (control:AbstractControl):null => {
    const password = control.get('password')
    const confirmPassword = control.get('confirmPassword')

    if(password && confirmPassword && password.value != confirmPassword.value)
      confirmPassword?.setErrors({passWordMissMatch:true})
    else
      confirmPassword?.setErrors(null)

    return null;
  }

  form = this.formBuilder.group({
    fullName : ['', Validators.required],
    userName : ['', Validators.required],
    email : ['', [Validators.required, Validators.email]],
    password : ['', [Validators.required, Validators.minLength(3)]],
    confirmPassword : [''],
  },{validators:this.passWordMatchValid})

  onSubmit() {
    this.isSubmitted = true;

    if (this.form.valid) {
      this.service.createUser(this.form.value).subscribe({
        next: (res: any) => {
          if (res.succeeded) {
            this.form.reset();
            this.isSubmitted = false;
            this.toastr.success('New User Created..!','Registration Successfull');
            this.router.navigateByUrl('/login');
          }
        },
        error: (err) => {
          if (err.error.errors)
            err.error.errors.forEach((x: any) => {
              switch (x.code) {
                case 'DuplicateUserName':
                  this.toastr.error('User Name is already taken..!','Registration Failed');
                  break;
                case 'DuplicateEmail':
                  this.toastr.error('Email is already taken..!','Registration Failed');
                  break;
                default:
                  this.toastr.error('Contact the developer','Registration Failed');
                  console.log(x);
                  break;
              }
            });
          else console.log('error', err);
        },
      });
    }
  }

  hasDisplayableError(controlName: string): Boolean {
    const control = this.form.get(controlName);
    return (
      Boolean(control?.invalid) &&
      (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty))
    );
  }
}
