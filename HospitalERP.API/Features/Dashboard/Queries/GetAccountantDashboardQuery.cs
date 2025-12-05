using HospitalERP.API.Features.Dashboard.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Dashboard.Queries;

public record GetAccountantDashboardQuery : IRequest<AccountantDashboardDto>;

