import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'app-patient-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, CardModule, ButtonModule, TagModule, RippleModule],
  templateUrl: './patient-detail.html',
  styleUrl: './patient-detail.css'
})
export class PatientDetailComponent {
  patient = {
    id: 1,
    firstName: 'John',
    lastName: 'Doe',
    dateOfBirth: '1985-05-15',
    gender: 'Male',
    bloodType: 'O+',
    contactNumber: '555-0101',
    address: '123 Main St, City, State 12345'
  };

  appointments = [
    { id: 1, date: '2024-01-15', doctor: 'Dr. Smith', service: 'General Checkup', status: 'Completed' },
    { id: 2, date: '2024-02-20', doctor: 'Dr. Johnson', service: 'Cardiology', status: 'Scheduled' }
  ];

  medicalRecords = [
    { id: 1, date: '2024-01-15', diagnosis: 'Hypertension', doctor: 'Dr. Smith' },
    { id: 2, date: '2023-12-10', diagnosis: 'Common Cold', doctor: 'Dr. Johnson' }
  ];
}

