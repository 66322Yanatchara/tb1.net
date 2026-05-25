# Frontend API Endpoints

Base URL

```text
Local:  http://localhost:5030
Render: https://tb1-net.onrender.com
```

Swagger

```text
GET /swagger/index.html
GET /swagger/v1/swagger.json
```

## 1. Authentication Module

Use for login screen, current-user state, and future JWT flow.

| Method | Endpoint | Frontend Use | Status |
| --- | --- | --- | --- |
| POST | `/api/auth/login` | Login form | Partial |
| POST | `/api/auth/refresh` | Refresh access token | Placeholder |
| POST | `/api/auth/logout` | Logout action | Partial |
| GET | `/api/auth/me` | Load current user profile | Placeholder |

Login body:

```json
{
  "username": "admin",
  "password": "password"
}
```

Note: JWT generation and password hashing are not wired yet. The login endpoint checks active user data but does not issue a real token yet.

## 2. Users, Roles, Permissions

Use for user management and permission screens.

| Method | Endpoint | Frontend Use |
| --- | --- | --- |
| GET | `/api/users` | User list |
| GET | `/api/users/{id}` | User detail |
| POST | `/api/users` | Create user |
| PUT | `/api/users/{id}` | Update user |
| DELETE | `/api/users/{id}` | Delete user |
| GET | `/api/roles` | Role list |
| POST | `/api/roles` | Create role |
| GET | `/api/permissions` | Permission list |
| POST | `/api/permissions` | Create permission |
| POST | `/api/roles/{roleId}/permissions` | Assign permission to role |

Create user body:

```json
{
  "username": "employee01",
  "passwordHash": "hashed-password",
  "employeeId": "00000000-0000-0000-0000-000000000000",
  "isActive": true
}
```

Assign permission body:

```json
{
  "permissionId": "00000000-0000-0000-0000-000000000000"
}
```

## 3. Organization Module

Use for organization tree, PEA area/branch, department, and position screens.

### Organizations

| Method | Endpoint | Frontend Use |
| --- | --- | --- |
| GET | `/api/organizations` | Organization list/tree source |
| GET | `/api/organizations/{id}` | Organization detail |
| POST | `/api/organizations` | Create organization |
| PUT | `/api/organizations/{id}` | Update organization |
| DELETE | `/api/organizations/{id}` | Delete organization |

Organization body:

```json
{
  "code": "PEA-001",
  "name": "กฟจ. ตัวอย่าง",
  "type": "province",
  "parentId": null
}
```

### Departments

| Method | Endpoint | Frontend Use |
| --- | --- | --- |
| GET | `/api/departments` | Department dropdown/list |
| POST | `/api/departments` | Create department |
| PUT | `/api/departments/{id}` | Update department |
| DELETE | `/api/departments/{id}` | Delete department |

Department body:

```json
{
  "organizationId": "00000000-0000-0000-0000-000000000000",
  "name": "แผนกความปลอดภัย"
}
```

### Positions

| Method | Endpoint | Frontend Use |
| --- | --- | --- |
| GET | `/api/positions` | Position dropdown/list |
| POST | `/api/positions` | Create position |

Position body:

```json
{
  "name": "เจ้าหน้าที่ความปลอดภัย"
}
```

## 4. Employee Module

Use for employee/contractor management, search, and filters.

| Method | Endpoint | Frontend Use |
| --- | --- | --- |
| GET | `/api/employees` | Employee list |
| GET | `/api/employees/{id}` | Employee detail |
| POST | `/api/employees` | Create employee |
| PUT | `/api/employees/{id}` | Update employee |
| DELETE | `/api/employees/{id}` | Delete employee |
| GET | `/api/employees/search?keyword={keyword}` | Search employee |
| GET | `/api/employees/by-organization/{orgId}` | Filter by organization |
| GET | `/api/employees/by-department/{departmentId}` | Filter by department |

Employee body:

```json
{
  "employeeCode": "E001",
  "firstName": "Somchai",
  "lastName": "Jaidee",
  "email": "somchai@example.com",
  "organizationId": "00000000-0000-0000-0000-000000000000",
  "departmentId": "00000000-0000-0000-0000-000000000000",
  "positionId": "00000000-0000-0000-0000-000000000000",
  "employeeType": "employee"
}
```

## 5. Safety Talk Event Module

Use for event CRUD, registration, participant list, and GPS config.

### Events

| Method | Endpoint | Frontend Use |
| --- | --- | --- |
| GET | `/api/events` | Event list |
| GET | `/api/events/{id}` | Event detail |
| POST | `/api/events` | Create event |
| PUT | `/api/events/{id}` | Update event |
| DELETE | `/api/events/{id}` | Delete event |

Event body:

```json
{
  "title": "Safety Talk ประจำเดือน",
  "description": "หัวข้อความปลอดภัย",
  "startTime": "2026-05-25T09:00:00Z",
  "endTime": "2026-05-25T10:00:00Z",
  "createdBy": "00000000-0000-0000-0000-000000000000"
}
```

### Event Registration

| Method | Endpoint | Frontend Use |
| --- | --- | --- |
| POST | `/api/events/{eventId}/register` | Register employee |
| DELETE | `/api/events/{eventId}/register/{employeeId}` | Remove registration |
| GET | `/api/events/{eventId}/participants` | Participant list |

Register body:

```json
{
  "employeeId": "00000000-0000-0000-0000-000000000000"
}
```

## 6. GPS Config Module

Use for setting event check-in location, radius, and allowed time.

| Method | Endpoint | Frontend Use |
| --- | --- | --- |
| GET | `/api/events/{eventId}/gps-config` | Load GPS config |
| POST | `/api/events/{eventId}/gps-config` | Create GPS config |
| PUT | `/api/events/{eventId}/gps-config` | Update GPS config |

GPS config body:

```json
{
  "latitude": 18.1234567,
  "longitude": 99.1234567,
  "radiusMeters": 100,
  "checkinStart": "2026-05-25T08:30:00Z",
  "checkinEnd": "2026-05-25T10:30:00Z"
}
```

## 7. Check-in Module

Use for mobile/web check-in screen. This module already validates duplicate, time window, GPS radius, and stores selfie URL.

| Method | Endpoint | Frontend Use |
| --- | --- | --- |
| POST | `/api/checkins` | Submit check-in |
| POST | `/api/checkins/validate-location` | Validate GPS distance |
| POST | `/api/checkins/validate-time` | Validate time window |
| POST | `/api/checkins/validate-duplicate` | Validate duplicate check-in |
| GET | `/api/checkins/event/{eventId}` | Event attendance list |
| GET | `/api/checkins/employee/{employeeId}` | Employee check-in history |

Check-in body:

```json
{
  "eventId": "00000000-0000-0000-0000-000000000000",
  "employeeId": "00000000-0000-0000-0000-000000000000",
  "latitude": 18.1234567,
  "longitude": 99.1234567,
  "selfiePhotoUrl": "https://example.com/selfie.jpg"
}
```

Validate location body:

```json
{
  "eventId": "00000000-0000-0000-0000-000000000000",
  "latitude": 18.1234567,
  "longitude": 99.1234567
}
```

Validate time body:

```json
{
  "eventId": "00000000-0000-0000-0000-000000000000"
}
```

Validate duplicate body:

```json
{
  "eventId": "00000000-0000-0000-0000-000000000000",
  "employeeId": "00000000-0000-0000-0000-000000000000"
}
```

## 8. Dashboard Module

Use for dashboard cards, attendance matrix, and organization/event summary widgets.

| Method | Endpoint | Frontend Use |
| --- | --- | --- |
| GET | `/api/dashboard/summary` | Dashboard KPI cards |
| GET | `/api/dashboard/attendance-matrix` | Attendance matrix |
| GET | `/api/dashboard/organization-summary` | Organization summary chart/table |
| GET | `/api/dashboard/event-summary/{eventId}` | Event summary detail |

## 9. Reporting Module

Use for report pages and export actions.

| Method | Endpoint | Frontend Use | Status |
| --- | --- | --- | --- |
| GET | `/api/reports/events` | Event report | Ready |
| GET | `/api/reports/attendance` | Attendance report | Ready |
| GET | `/api/reports/attendance?eventId={eventId}` | Attendance report by event | Ready |
| GET | `/api/reports/export/excel` | Export Excel | Placeholder |
| GET | `/api/reports/export/pdf` | Export PDF | Placeholder |

## 10. Audit Log Module

Use for audit log table/detail screen.

| Method | Endpoint | Frontend Use |
| --- | --- | --- |
| GET | `/api/audit-logs` | Audit log list |
| GET | `/api/audit-logs/{id}` | Audit log detail |

## 11. Upload Module

Use for photo/selfie upload flow. Current endpoints are placeholders until storage is configured.

| Method | Endpoint | Frontend Use | Status |
| --- | --- | --- | --- |
| POST | `/api/uploads/photo` | Upload general photo | Placeholder |
| POST | `/api/uploads/selfie` | Upload check-in selfie | Placeholder |

## 12. Health Check

Use for deployment and frontend connectivity checks.

| Method | Endpoint | Frontend Use |
| --- | --- | --- |
| GET | `/api/health` | API/database health check |

## Suggested Frontend Pages

| Page | Main Endpoints |
| --- | --- |
| Login | `POST /api/auth/login` |
| Dashboard | `GET /api/dashboard/summary`, `GET /api/dashboard/attendance-matrix` |
| Organizations | `/api/organizations`, `/api/departments`, `/api/positions` |
| Employees | `/api/employees`, `/api/employees/search`, employee filters |
| Safety Events | `/api/events`, `/api/events/{eventId}/participants`, `/api/events/{eventId}/gps-config` |
| Check-in | `POST /api/checkins`, validation endpoints |
| Reports | `/api/reports/events`, `/api/reports/attendance` |
| Audit Logs | `/api/audit-logs` |
| User Management | `/api/users`, `/api/roles`, `/api/permissions` |
