import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DatePickerModule } from 'primeng/datepicker';
import { SelectModule } from 'primeng/select';
import { TextareaModule } from 'primeng/textarea';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'app-patient-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    CardModule,
    ButtonModule,
    InputTextModule,
    DatePickerModule,
    SelectModule,
    TextareaModule,
    RippleModule
  ],
  templateUrl: './patient-form.html',
  styleUrl: './patient-form.css'
})
export class PatientFormComponent {
  patient = {
    firstName: '',
    lastName: '',
    dateOfBirth: null as Date | null,
    gender: null as any,
    bloodType: null as any,
    contactNumber: '',
    address: ''
  };

  genders = [
    { label: 'Male', value: 'Male' },
    { label: 'Female', value: 'Female' }
  ];

  bloodTypes = [
    { label: 'O+', value: 'O+' },
    { label: 'O-', value: 'O-' },
    { label: 'A+', value: 'A+' },
    { label: 'A-', value: 'A-' },
    { label: 'B+', value: 'B+' },
    { label: 'B-', value: 'B-' },
    { label: 'AB+', value: 'AB+' },
    { label: 'AB-', value: 'AB-' }
  ];

  onSubmit() {
    console.log('Form submitted:', this.patient);
    // Will be implemented in Phase 2
  }
}

