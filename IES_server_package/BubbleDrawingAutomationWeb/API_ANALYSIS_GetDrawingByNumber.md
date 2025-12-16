# API Analysis: GetDrawingByNumber

## 📍 Endpoint Details

**URL:** `http://localhost:44436/api/drawingsearch/GetDrawingByNumber`  
**Method:** `POST` (HTTP POST - Private/Internal API)  
**Controller:** `DrawingSearchController.cs`  
**Route:** `[Route("GetDrawingByNumber")]`  
**Line:** 2325-2690

---

## 🔄 Navigation Flow: Public vs Private POST

### **API Type: PRIVATE POST (Internal API)**

This is **NOT a public API**. It's an internal API endpoint that:
- ✅ Requires session management
- ✅ Uses server-side session storage
- ✅ Called from React frontend via proxy
- ✅ Protected by CORS policies configured in `appsettings.json`

### **Request Flow:**

```
┌─────────────────┐
│  React Frontend │
│  (Common.js)    │
└────────┬────────┘
         │
         │ POST /api/drawingsearch/GetDrawingByNumber
         │ Body: SearchForm { drawingNo, revNo, baseUrl, sessionUserId }
         ▼
┌─────────────────┐
│  setupProxy.js  │
│  (Proxy Config) │
└────────┬────────┘
         │
         │ Proxies to: http://localhost:44436
         │ Timeout: 30 minutes
         ▼
┌─────────────────────────┐
│  DrawingSearchController │
│  GetDrawingByNumber()    │
└────────┬────────────────┘
         │
         │ 1. Session Management
         │ 2. Load Configuration
         │ 3. Environment Check
         │ 4. Fetch Drawing Images
         │ 5. Process & Return
         ▼
┌─────────────────┐
│  Response       │
│  (JSON Array)   │
└─────────────────┘
```

---

## 📥 Request Structure

### **Frontend Call (Common.js:32-68)**

```javascript
// Location: ClientApp/src/components/Common/Common.js

export async function fetchSearchData(state) {
    let url = "/api/drawingsearch/GetDrawingByNumber";
    await fetchConfig();
    
    // Uses axios instance with baseURL configuration
    const res = await instance.post(url, state);
    
    // State object structure:
    // {
    //   drawingNo: string,
    //   revNo: string,
    //   baseUrl: string,
    //   sessionUserId: string
    // }
}
```

### **Request Model (SearchForm)**

```csharp
public class SearchForm
{
    public string drawingNo { get; set; }      // Drawing number to search
    public string revNo { get; set; }          // Revision number
    public string baseUrl { get; set; }        // Base URL (optional)
    public string sessionUserId { get; set; }  // Session user ID (optional)
}
```

---

## 🔐 Session Management Flow

### **Session Object Structure**

```csharp
// Location: SessionVariables.cs
public class sessionobj
{
    public string sessionUserName { get; set; }  // "Current User"
    public string sessionUserId { get; set; }   // GUID (e.g., "a1b2c3d4-...")
    public TimeSpan expire { get; set; }        // 10 minutes
}
```

### **Session Flow in API:**

```csharp
// Step 1: Get existing session
sessionobj sessionData = getsession();

// Step 2: If no session exists, create new one
if (sessionData == null)
{
    sessionData = setsession();  // Creates new GUID session
}

// Step 3: Use sessionUserId for file paths (production only)
if (env != "development")
{
    clientPath = clientPath + sessionUserId + "\\";
    // Creates user-specific directory for images
}
```

### **Session Methods:**

**`getsession()`** (Line 869-874)
- Retrieves session from `HttpContext.Session`
- Returns `sessionobj` or `null` if not exists

**`setsession()`** (Line 875-896)
- Creates new session if doesn't exist
- Generates new GUID for `sessionUserId`
- Sets expiration to 10 minutes
- Updates existing session expiration if already exists

---

## 🏗️ API Implementation Flow

### **Step-by-Step Execution:**

#### **1. Initial Setup (Lines 2328-2344)**
```csharp
try {
    // Get/Create session
    sessionobj sessionData = getsession();
    if (sessionData == null) {
        sessionData = setsession();
    }
    
    // Initialize data tables
    dtFiles_Header = RequestHeader(dtFiles_Header);
    dtFiles_Production = RequestProduct(dtFiles_Production);
    
    // Check configuration exists
    if (_dbcontext.TblConfigurations == null) {
        return NotFound();
    }
}
```

#### **2. Environment Check (Lines 2345-2542)**

**Production Environment:**
```csharp
if (env != "development")
{
    // 1. Create user-specific directory
    clientPath = clientPath + sessionUserId + "\\";
    
    // 2. Call External API
    externalApiUrl = "https://mfgdweb001.corp.halliburton.com/SCGSrv/DrawingUrl/GetDrawingServiceUrlList"
    endPoint = "/{drawingNo}/{revNo}"
    
    // 3. Download images from external service
    using (HttpClient httpClient = new HttpClient())
    {
        HttpResponseMessage res = httpClient.GetAsync(externalApiUrl + endPoint);
        // Parse JSON response
        // Download each image file
        // Save to user-specific directory
    }
}
```

**Development Environment:**
```csharp
else
{
    // 1. Query MPM Database
    connstr = getConnectionStr(dtMPM);
    query = "SELECT ProductionOrderNo, Drawing_No, DRevNo, PartNo, PRevNo 
             FROM Tbl_BalloonDrawing 
             WHERE Drawing_No = @Drawing_No AND DRevNo = @DRevNo";
    
    // 2. Search local file system
    var myFiles = Directory.GetFiles(objSettings.DrawingFolder, 
                                      search.Trim() + ".*");
    
    // 3. Process local image files
    // Convert TIFF to PNG if needed
    // Scale images
}
```

#### **3. Image Processing (Lines 2475-2511)**

```csharp
// For each image file:
// 1. Copy to temp location
// 2. Process rotation (if needed)
// 3. Copy to client path
// 4. Scale image for display
// 5. Extract page/revision details
```

#### **4. Database Query (Lines 2512-2520)**

```csharp
// Fetch existing balloon data
IEnumerable<object> results = _dbcontext.TblBaloonDrawingLiners
    .Where(w => w.DrawingNumber == searchForm.drawingNo && 
                w.Revision == searchForm.revNo)
    .OrderBy(f => f.DrawLineID)
    .ToList();
```

#### **5. Response Assembly (Lines 2517-2529)**

```csharp
var returnObject = new List<object>();
returnObject.Add(dtFiles_Production);      // Image files data
returnObject.Add(dtFiles_Header);          // Drawing header info
returnObject.Add(results);                  // Existing balloon data
returnObject.Add(lmtype);                   // Measure types
returnObject.Add(lmsubtype);                // Measure sub-types
returnObject.Add(new object[] { "Inches" }); // Units
returnObject.Add(new object[] { "Linear", "Default" }); // Tolerance types
returnObject.Add(partial_image);            // Partial image data

return StatusCode(StatusCodes.Status200OK, returnObject);
```

---

## 📤 Response Structure

### **Success Response (200 OK):**

Returns an **array of 8 objects**:

```javascript
[
    0: dtFiles_Production,    // DataTable: Image files with paths, page numbers
    1: dtFiles_Header,        // DataTable: Drawing header (DrawingNo, Revision, PartNo)
    2: results,               // Array: Existing balloon annotations from DB
    3: lmtype,                // DataTable: Measure types (Dimension, Notes, etc.)
    4: lmsubtype,             // DataTable: Measure sub-types (Linear, Circularity, etc.)
    5: ["Inches"],            // Array: Units
    6: ["Linear", "Default"], // Array: Tolerance types
    7: partial_image          // Array: Partial image metadata
]
```

### **Error Responses:**

- **404 NotFound:** Configuration not found
- **204 NoContent:** Drawing doesn't exist
- **400 BadRequest:** Invalid inputs or exception occurred

---

## 🔒 Security & Access Control

### **CORS Configuration:**

```json
// appsettings.json
"CorsPolicies": {
    "halliburtonprod": {
        "Origins": [
            "http://ap1mfgweb.corp.halliburton.com:82",
            "http://localhost:44436",
            "https://ap1mfgweb.corp.halliburton.com:444"
        ]
    }
}
```

### **Session Security:**
- ✅ Session-based authentication
- ✅ 10-minute session expiration
- ✅ User-specific file directories (production)
- ✅ Session stored in server memory (DistributedMemoryCache)

### **No Public Access:**
- ❌ No anonymous access
- ❌ Requires valid session
- ❌ Protected by CORS policies
- ❌ Internal API only (not exposed publicly)

---

## 🔄 Complete Navigation Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                    FRONTEND (React)                          │
│                                                              │
│  User enters Drawing No & Revision                          │
│         │                                                    │
│         ▼                                                    │
│  fetchSearchData(state)                                      │
│  ├─ drawingNo: "59v701081"                                   │
│  ├─ revNo: "A"                                              │
│  ├─ baseUrl: ""                                             │
│  └─ sessionUserId: "" (optional)                            │
│         │                                                    │
│         ▼                                                    │
│  instance.post("/api/drawingsearch/GetDrawingByNumber")     │
└────────────────────────┬────────────────────────────────────┘
                         │
                         │ HTTP POST
                         │ (via setupProxy.js)
                         ▼
┌─────────────────────────────────────────────────────────────┐
│              PROXY MIDDLEWARE (setupProxy.js)               │
│                                                              │
│  - Proxies to: http://localhost:44436                       │
│  - Timeout: 30 minutes                                       │
│  - CORS headers added                                        │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────────┐
│         BACKEND API (DrawingSearchController)               │
│                                                              │
│  1. Session Management                                       │
│     ├─ getsession() → Check existing session                │
│     └─ setsession() → Create new if needed                  │
│                                                              │
│  2. Configuration Load                                       │
│     └─ LoadConfigDetails() → Load DB configs                 │
│                                                              │
│  3. Environment Check                                        │
│     ├─ Production: Call external API                        │
│     │   └─ Download images from Halliburton service         │
│     └─ Development: Query local DB & file system            │
│                                                              │
│  4. Image Processing                                         │
│     ├─ Download/Copy images                                 │
│     ├─ Convert TIFF → PNG                                    │
│     ├─ Scale images                                         │
│     └─ Extract page/revision info                           │
│                                                              │
│  5. Database Query                                           │
│     └─ Fetch existing balloon annotations                   │
│                                                              │
│  6. Response Assembly                                        │
│     └─ Return 8-object array                                │
└────────────────────────┬────────────────────────────────────┘
                         │
                         │ JSON Response
                         ▼
┌─────────────────────────────────────────────────────────────┐
│              FRONTEND (React) - Response Handling            │
│                                                              │
│  Process response array:                                     │
│  ├─ Update drawingDetails state                             │
│  ├─ Load images to canvas                                   │
│  ├─ Load existing balloons                                  │
│  └─ Initialize UI components                                │
└─────────────────────────────────────────────────────────────┘
```

---

## 📝 Key Points

### **Public vs Private:**
- **PRIVATE API** - Not publicly accessible
- Requires session management
- Called internally from React app
- Protected by CORS and session middleware

### **Session Management:**
- **Automatic session creation** if doesn't exist
- **10-minute expiration** (auto-refreshed on each call)
- **User-specific directories** in production (using sessionUserId)
- **Session stored in server memory** (DistributedMemoryCache)

### **Environment Differences:**
- **Production:** Downloads from external Halliburton API
- **Development:** Uses local file system and database

### **Response Format:**
- Returns **array of 8 objects** (not a single object)
- First 2 are DataTables (converted to JSON)
- Remaining are arrays/objects

---

## 🐛 Error Handling

```csharp
try {
    // Main logic
}
catch (Exception ex) {
    objerr.WriteErrorToText(ex);  // Log to file
    return BadRequest("Oops!.. Something Went wrong.Please Try later.");
}
```

**Error Log Location:** `ErrorLog` folder in application directory

---

## 🔍 Related Files

1. **Controller:** `Controllers/DrawingSearchController.cs` (Line 2325-2690)
2. **Frontend Call:** `ClientApp/src/components/Common/Common.js` (Line 32-68)
3. **Proxy Config:** `ClientApp/src/setupProxy.js` (Line 10)
4. **Session Model:** `SessionVariables.cs`
5. **Request Model:** `Controllers/DrawingSearchController.cs` (Line 56-63)

---

## 📊 Summary

| Aspect | Details |
|--------|---------|
| **API Type** | Private POST (Internal) |
| **Authentication** | Session-based |
| **Session Duration** | 10 minutes |
| **Timeout** | 30 minutes (proxy) |
| **Response Format** | Array of 8 objects |
| **Environment Support** | Production & Development |
| **External Dependencies** | Halliburton Drawing Service (Production) |
| **File Storage** | User-specific directories (Production) |

