import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';

@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './registration.component.html',
  styles: ``
})

export class RegistrationComponent {
  constructor(public formBuilder: FormBuilder) {}
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
    email : ['', [Validators.required, Validators.email]],
    password : ['', [Validators.required, Validators.minLength(3)]],
    confirmPassword : [''],
  },{validators:this.passWordMatchValid})

  onSubmit(){
    this.isSubmitted = true;
    console.log(this.form.value);
  }

  hasDisplayableError(controlName: string):Boolean{
    const control = this.form.get(controlName);
    return Boolean(control?.invalid) && (this.isSubmitted || Boolean(control?.touched))
  }
}
