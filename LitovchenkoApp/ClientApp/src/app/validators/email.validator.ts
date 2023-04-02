import { AbstractControl, ValidationErrors } from '@angular/forms';

export class EmailValidator {
    public static validEmail(control:AbstractControl) : ValidationErrors | null {
        var value = control.value;
        if (!value){
            return null;
        }
        const isValidEmail = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/.test(value);
        return isValidEmail ? null : {isValidEmail : isValidEmail};
    }
}