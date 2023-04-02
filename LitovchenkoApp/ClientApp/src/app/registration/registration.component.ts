import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { PasswordValidator } from '../validators/password.validator';
import { EmailValidator } from '../validators/email.validator';
import { MatStepper } from '@angular/material/stepper';
import {MatSnackBar} from '@angular/material/snack-bar';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html'
})
export class RegistrationComponent{
    countries: Country[] = [];
    provinces: Province[] = [];
    registrationForm: FormGroup;
    step1Form : FormGroup;
    step2Form : FormGroup;
    hidePassword = true;
    registrationError: string = '';
  
    constructor(private snackBar: MatSnackBar, private http: HttpClient, builder: FormBuilder, @Inject('BASE_URL') private baseUrl: string) {
      http.get<Country[]>(baseUrl + 'api/countries').subscribe(result => {
        this.countries = result;
      }, error => console.error(error));

      this.step1Form = builder.group({
        email: ['', [Validators.required, EmailValidator.validEmail]],
        password: ['', [Validators.required, PasswordValidator.strong]],
        confirmPassword: ['', [Validators.required, PasswordValidator.confirmEquals]],
        foodWorkCheck: [false, [Validators.requiredTrue]],
      });
      this.step2Form = builder.group({
        country: ['', Validators.required],
        province: ['', Validators.required]
      });
      this.registrationForm = builder.group({
        step1Form: this.step1Form,
        step2Form: this.step2Form
      });
    }

    onSubmit(stepper: MatStepper){
      const user = {
        email: this.step1Form.get('email')?.value,
        password: this.step1Form.get('password')?.value,
        provinceId: this.step2Form.get('province')?.value
      }
      this.http.post(this.baseUrl + 'api/registration', user).subscribe(result => {
        console.log('done');
        this.registrationError = '';
        this.snackBar.open(`${result} successfully registered`, "Close", {
          duration: 5000,
        });
      }, error => {
        console.log(error);
        stepper.previous();
        this.registrationError = error.error.message
      });
    }

    getProvinces() {
      const currentCountryId = this.step2Form.get('country')
      if (!currentCountryId?.value){
        return;
      }
      this.http.get<Province[]>(this.baseUrl + 'api/provinces', {params : new HttpParams().set('countryId', currentCountryId.value)}).subscribe(result => {
        this.provinces = result;
      }, error => {console.log(error)})
    }

    getPasswordErrorMessage(controlName : string) : string {
      return PasswordValidator.getErrorMessage(this.step1Form.get(controlName))
    }

}

interface Country {
    id: number
    name: string;
    provinces: Province[];
}

interface Province {
    id: number;
    name : string;
    countryId: number;
}

