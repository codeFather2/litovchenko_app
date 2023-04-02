import { AbstractControl, ValidationErrors } from '@angular/forms';

export class PasswordValidator {

    public static strong(control:AbstractControl) : ValidationErrors | null {

        const value = control.value;

        if (!value) {
            return null;
        }

        const short = value.length < 2;
        const hasLetter = /[A-Za-z]+/.test(value);

        const hasNumeric = /[0-9]+/.test(value);

        const passwordValid = hasLetter && hasNumeric;

        return !passwordValid ? {noLetter:!hasLetter, noNumeric: !hasNumeric, tooShort: short}: null;
    }

    public static getErrorMessage(validation: ValidationErrors | null) : string {
        if (!validation || !validation.errors){
            return '';
        }
        const errors = validation.errors;
        if (errors.notEqual) {
            return "Passwords should be equal"
        }
        if (errors.required){
            return "Please enter the password";
        }
        const commonPart = 'password should contain at least';
        if (errors.tooShort) {
            return commonPart + ' 2 symbols';
        }
        if (errors.noLetter) {
            return commonPart + ' one letter';
        }
        if (errors.noNumeric) {
            return commonPart + ' one number';
        }
        return '';
    }

    public static confirmEquals(control: AbstractControl): { [key: string]: any } | null {
        const password = control.parent?.get('password');
        const confirmPassword = control;
        if (password && confirmPassword && password.value !== confirmPassword.value) {
            return { notEqual : true };
        }
        return null;
    }
}