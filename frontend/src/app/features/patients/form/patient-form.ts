import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DatePickerModule } from 'primeng/datepicker';
import { SelectModule } from 'primeng/select';
import { TextareaModule } from 'primeng/textarea';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { PatientsService } from '../services/patients.service';
import { CreatePatientModel } from '../models/create-patient.model';
import { UpdatePatientModel } from '../models/update-patient.model';

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
    RippleModule,
    ToastModule
  ],
  providers: [MessageService],
  templateUrl: './patient-form.html',
  styleUrl: './patient-form.css'
})
export class PatientFormComponent implements OnInit {
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private patientsService = inject(PatientsService);
  private messageService = inject(MessageService);

  isSubmitting = false;
  isEditMode = false;
  isLoading = false;
  patientId: number | null = null;

  patient = {
    firstName: '',
    lastName: '',
    dateOfBirth: null as Date | null,
    genderID: null as number | null,
    bloodTypeID: null as number | null,
    contactNumber: '',
    address: ''
  };

  genders = [
    { label: 'Male', value: 1 },
    { label: 'Female', value: 2 },
    { label: 'Other', value: 3 }
  ];

  bloodTypes = [
    { label: 'A+', value: 1 },
    { label: 'A-', value: 2 },
    { label: 'B+', value: 3 },
    { label: 'B-', value: 4 },
    { label: 'AB+', value: 5 },
    { label: 'AB-', value: 6 },
    { label: 'O+', value: 7 },
    { label: 'O-', value: 8 }
  ];

  ngOnInit() {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.patientId = +params['id'];
        this.loadPatientData();
      }
    });
  }

  loadPatientData() {
    if (!this.patientId) return;

    this.isLoading = true;
    this.patientsService.getById(this.patientId).subscribe({
      next: (data) => {
        this.patient = {
          firstName: data.firstName,
          lastName: data.lastName,
          dateOfBirth: new Date(data.dateOfBirth),
          genderID: data.genderID,
          bloodTypeID: data.bloodTypeID,
          contactNumber: data.contactNumber,
          address: data.address
        };
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading patient:', error);
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load patient data' });
        this.isLoading = false;
      }
    });
  }

  onSubmit() {
    if (this.isSubmitting) return;

    // Basic validation
    if (!this.patient.firstName || !this.patient.lastName || !this.patient.dateOfBirth ||
      !this.patient.genderID || !this.patient.bloodTypeID || !this.patient.contactNumber || !this.patient.address) {
      this.messageService.add({ severity: 'warn', summary: 'Validation', detail: 'Please fill in all required fields.' });
      return;
    }

    this.isSubmitting = true;

    // Format date to YYYY-MM-DD
    const formattedDate = this.patient.dateOfBirth ?
      new Date(this.patient.dateOfBirth).toISOString().split('T')[0] : '';

    if (this.isEditMode && this.patientId) {
      // Update existing patient
      const model: UpdatePatientModel = {
        patientID: this.patientId,
        firstName: this.patient.firstName,
        lastName: this.patient.lastName,
        dateOfBirth: formattedDate,
        genderID: this.patient.genderID,
        bloodTypeID: this.patient.bloodTypeID,
        contactNumber: this.patient.contactNumber,
        address: this.patient.address
      };

      this.patientsService.update(this.patientId, model).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Patient updated successfully' });
          setTimeout(() => this.router.navigate(['/patients', this.patientId]), 1000);
        },
        error: (error) => {
          console.error('Error updating patient:', error);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error.message || 'Failed to update patient' });
          this.isSubmitting = false;
        }
      });
    } else {
      // Create new patient
      const model: CreatePatientModel = {
        firstName: this.patient.firstName,
        lastName: this.patient.lastName,
        dateOfBirth: formattedDate,
        genderID: this.patient.genderID,
        bloodTypeID: this.patient.bloodTypeID,
        contactNumber: this.patient.contactNumber,
        address: this.patient.address
      };

      this.patientsService.create(model).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Patient created successfully' });
          setTimeout(() => this.router.navigate(['/patients']), 1000);
        },
        error: (error) => {
          console.error('Error creating patient:', error);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error.message || 'Failed to create patient' });
          this.isSubmitting = false;
        }
      });
    }
  }
}
