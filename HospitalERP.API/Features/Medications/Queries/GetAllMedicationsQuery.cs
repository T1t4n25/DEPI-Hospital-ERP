using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Features.Medications.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Medications.Queries;

public record GetAllMedicationsQuery(QueryParams QueryParams) : IRequest<PaginatedResponse<MedicationListDto>>;

