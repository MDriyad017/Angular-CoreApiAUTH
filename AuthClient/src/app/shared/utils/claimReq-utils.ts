export const claimReq = {
    adminOnly: (c: any) => c.role == "Admin",
    adminOrFactoryManager: (c: any) => c.role == "Admin" || c.role == "Factory Manager",
    hasLocationId: (c: any) => 'locationId' in c,
    femaleAndAdmin: (c: any) => c.gender == "Female" || c.role == "Admin"
}