import { CreateServiceModel } from './create-service.model';

export interface UpdateServiceModel extends CreateServiceModel {
  serviceID: number;
}

