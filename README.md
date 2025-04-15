# **Atlas Excel Add-in – Function Reference**

The **Atlas Excel Add-in** provides powerful geographic and spatial analysis functions directly within Excel, enabling quick insights and analysis for any organization.

---

## 🧩 Installation Options

Download the latest version from the [Releases](https://github.com/andresharpe/AtlasGeoAddin/releases).

### 🔧 Manual Installation
1. Download the zip file (e.g. `AtlasAddIn-v1.2.3.zip`) from the release.
2. Extract it.
3. Open Excel → `File` → `Options` → `Add-ins` → `Go...`.
4. Click `Browse`, select the `.xll`, and load it.

### 📦 Installer Method
1. From the same zip, run `AtlasAddInInstaller-v1.2.3.exe`.
2. The add-in will be automatically installed into Excel.


## **Distance & Measurement Functions**

### `GEO_DISTANCE(lat1, lon1, lat2, lon2, [unit])`
Calculates the distance between two geographic coordinates.
- `unit`: `"km"` (default), `"mi"`

**Example:**  
`=GEO_DISTANCE(51.5, -0.1, 40.7, -74)` *(London to NYC distance)*

---

### `GEO_TOTALDISTANCE(latlon_range, [unit])`
Calculates total sequential distance for a list of lat/lon points.
- `unit`: `"km"` (default), `"mi"`

---

## **Nearest & Furthest Lookup Functions**

### Single Item Lookup:
- **Nearest:** `GEO_LOOKUPNEAREST(lat, lon, lat_range, lon_range, id_range, return_distance)`
- **Furthest:** `GEO_LOOKUPFURTHEST(lat, lon, lat_range, lon_range, id_range, return_distance)`

Returns the identifier (`id_range`) of the nearest or furthest point.

---

### Multiple Item Lookup (K-nearest/furthest):
- **Nearest K:** `GEO_LOOKUPNEARESTK(lat, lon, lat_range, lon_range, id_range, [k], return_distance)`
- **Furthest K:** `GEO_LOOKUPFURTHESTK(lat, lon, lat_range, lon_range, id_range, [k], return_distance)`

Returns array of identifiers (`id_range`) sorted by proximity.

---

### Radius-based Lookup:
#### `GEO_LOOKUPWITHINRADIUS(lat, lon, lat_range, lon_range, radius, [unit])`
Returns identifiers within specified radius.

---

## **KD-Tree Indexed Lookup Functions**

Optimized for speed when working with large datasets.

### Creating and Managing Indexes:
#### `GEO_INDEXCREATE(index_name, id_range, lat_range, lon_range)`
Creates an optimized spatial index for fast lookups.

#### `GEO_INDEXCLEAR(index_name)`
Removes the specified spatial index from memory.

---

### Indexed Nearest Lookup:
- **Nearest Indexed:** `GEO_INDEXLOOKUPNEAREST(index_name, lat, lon, return_distance)`
- **Nearest Indexed:** `GEO_INDEXLOOKUPNEARESTK(index_name, lat, lon, [k], return_distance)`

Instantly returns IDs of nearest locations using indexed data.

---

## **Midpoint, Centroid & Area**

- `GEO_MIDPOINT(lat1, lon1, lat2, lon2)`
  Midpoint between two coordinates.
  
- `GEO_CENTROID(lat_range, lon_range)`
  Geographic center (centroid) of multiple coordinates.

---

## **Bearing & Direction**

- `GEO_BEARING(lat1, lon1, lat2, lon2)`
  Initial bearing between points (degrees).
  
- `GEO_DIRECTION(lat1, lon1, lat2, lon2)`
  Compass direction (N, NE, SW, etc.) between coordinates.

---

## **Validation & Formatting**

- `GEO_ISVALID(lat, lon)`
  Checks if coordinates are valid geographic coordinates.

- `GEO_NORMALIZE(lat, lon)`
  Normalizes latitude and longitude to standard ranges.

---

## **Conversion Functions**

### `GEO_TO_GOOGLE_MAPS_URL(lat, lon)`
Converts geographic coordinates to a Google Maps URL.

**Example:**  
`=GEO_TO_GOOGLE_MAPS_URL(51.5, -0.1)` *(Google Maps URL for London)*

---

## **Example Workflow for Optimized Lookups**

1. **Create a KD-tree index (once):**2. **Fast lookups using the index:**
3. **Clear index if no longer needed:**
---

## **Error Handling**
Atlas functions follow standard Excel conventions, returning Excel errors (`#N/A`, `#VALUE!`, `#NUM!`) clearly describing issues.

---

**Atlas Excel Add-in** provides robust, intuitive functions for your geographic analysis, directly integrated within Excel for efficiency, accuracy, and ease of use.

---

## **Summary of Functions**

- `GEO_CENTROID(lat_range, lon_range)`
- `GEO_DIRECTION(lat1, lon1, lat2, lon2)`
