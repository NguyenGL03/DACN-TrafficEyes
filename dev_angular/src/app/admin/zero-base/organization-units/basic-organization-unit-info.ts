export interface IBasicOrganizationUnitInfo {
    id: number;
    displayName: string;
    organizationCode: string;
    organizationType: string | number;
    organizationTypeName: string;
}

export enum OrganizationType {
    Branch = '1',
    Department = '2',
}