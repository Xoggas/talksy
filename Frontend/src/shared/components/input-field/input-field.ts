import {Component, forwardRef, Input} from '@angular/core';
import {ControlValueAccessor, NG_VALUE_ACCESSOR, ReactiveFormsModule} from '@angular/forms';

@Component({
  selector: 'app-input-field',
  imports: [
    ReactiveFormsModule
  ],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputField),
      multi: true,
    },
  ],
  templateUrl: './input-field.html',
  styleUrl: './input-field.css'
})
export class InputField implements ControlValueAccessor {
  @Input() type: string = "text";
  @Input() placeholder: string = "placeholder";
  @Input() icon: string = "empty_icon";

  value: any = '';
  onChange: any = () => {
  };
  onTouched: any = () => {
  };

  writeValue(value: any): void {
    this.value = value ?? '';
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
}
