# 🗺️ Atlas Excel Add-in – Function Reference

<p align="center">
  <img src="./docs/Atlas-01.png" alt="Atlas Logo" width="200"/>
</p>

The **Atlas Excel Add-in** brings advanced geographic and spatial analysis tools directly into Microsoft Excel. Perform fast, flexible, and intuitive geospatial computations without leaving your spreadsheet.

---

## 🚀 Installation

1. Download the latest version from the [Releases](https://github.com/andresharpe/AtlasGeoAddin/releases).
2. Extract the ZIP (e.g. `AtlasAddIn-v1.2.3.zip`).
3. Run `AtlasAddInInstaller-v1.2.3.exe`.
4. Excel will automatically register the add-in.

---

## 📏 Distance & Measurement

### `GEO_DISTANCE(lat1, lon1, lat2, lon2, [unit])`
Returns the great-circle distance between two coordinates using the Haversine formula.

- `lat1`, `lon1`: Coordinates of the first location.  
- `lat2`, `lon2`: Coordinates of the second location.  
- `unit`: *(Optional)* `"km"` (default) or `"mi"`.

**Example:**  
`=GEO_DISTANCE(51.5, -0.1, 40.7, -74)`

---

### `GEO_TOTALDISTANCE(latlon_range, [unit])`
Calculates total distance between sequential coordinate pairs in a range.

- `latlon_range`: 2-column range with latitudes and longitudes.  
- `unit`: *(Optional)* `"km"` (default) or `"mi"`.

---

## 📍 Nearest & Furthest Lookups

### `GEO_LOOKUPNEAREST(lat, lon, lat_range, lon_range, id_range, return_distance)`
Returns the ID of the closest location to the specified point.

- `lat`, `lon`: Origin coordinates.  
- `lat_range`, `lon_range`: Ranges of coordinates to compare.  
- `id_range`: Corresponding IDs for the locations.  
- `return_distance`: If `TRUE`, returns both ID and distance.

---

### `GEO_LOOKUPFURTHEST(lat, lon, lat_range, lon_range, id_range, return_distance)`
Same as above, but returns the furthest location.

---

### `GEO_LOOKUPNEARESTK(lat, lon, lat_range, lon_range, id_range, [k], return_distance)`
Returns a sorted list of the K nearest locations.

- `k`: *(Optional)* Number of results to return. Defaults to 1.  
Other arguments as above.

---

### `GEO_LOOKUPFURTHESTK(lat, lon, lat_range, lon_range, id_range, [k], return_distance)`
Same as above, but for furthest K locations.

---

### `GEO_LOOKUPWITHINRADIUS(lat, lon, lat_range, lon_range, radius, [unit])`
Returns a list of IDs within a given radius.

- `radius`: Distance threshold.  
- `unit`: *(Optional)* `"km"` (default) or `"mi"`.

---

## ⚡ KD-Tree Accelerated Lookups

### `GEO_INDEXCREATE(index_name, id_range, lat_range, lon_range)`
Creates a spatial index (KD-tree) in memory for faster queries.

- `index_name`: Unique name for the index.  
- `id_range`, `lat_range`, `lon_range`: Data to index.

---

### `GEO_INDEXCLEAR(index_name)`
Clears a named index from memory.

---

### `GEO_INDEXLOOKUPNEAREST(index_name, lat, lon, return_distance)`
Performs fast nearest neighbor lookup using a named index.

---

### `GEO_INDEXLOOKUPNEARESTK(index_name, lat, lon, [k], return_distance)`
Returns the K nearest IDs from the indexed dataset.

---

## 📌 Geometry Tools

### `GEO_MIDPOINT(lat1, lon1, lat2, lon2)`
Returns the geographic midpoint between two coordinates.

---

### `GEO_CENTROID(lat_range, lon_range)`
Computes the geographic center (centroid) of multiple coordinates.

---

## 🧭 Bearings & Compass Directions

### `GEO_BEARING(lat1, lon1, lat2, lon2)`
Returns the initial bearing from point A to point B in degrees.

---

### `GEO_DIRECTION(lat1, lon1, lat2, lon2)`
Returns compass direction (e.g., "N", "NE", "SW") from point A to B.

---

## ✅ Validation & Normalization

### `GEO_ISVALID(lat, lon)`
Checks whether the coordinates are valid (i.e., -90 ≤ lat ≤ 90, -180 ≤ lon ≤ 180).

---

### `GEO_NORMALIZE(lat, lon)`
Normalizes input to standard ranges:
- Latitude: -90 to 90  
- Longitude: -180 to 180

---

## 🔗 Conversion Utilities

### `GEO_TO_GOOGLE_MAPS_URL(lat, lon)`
Returns a shareable Google Maps link for the given coordinate.

**Example:**  
`=GEO_TO_GOOGLE_MAPS_URL(51.5, -0.1)`

---

## 🌍 Reverse Lookup Functions  

### `GEO_COUNTRY(lat, lon)`  
Finds the country name for a given latitude and longitude.  

- `lat`: The latitude value to lookup.  
- `lon`: The longitude value to lookup.  

**Example:**  
`=GEO_COUNTRY(51.5, -0.1)`  

---  

### `GEO_CITY(lat, lon)`  
Finds the city name for a given latitude and longitude.  

- `lat`: The latitude value to lookup.  
- `lon`: The longitude value to lookup.  

**Example:**  
`=GEO_CITY(51.5, -0.1)`  

---  

### `GEO_TIMEZONE(lat, lon)`  
Finds the timezone name for a given latitude and longitude.  

- `lat`: The latitude value to lookup.  
- `lon`: The longitude value to lookup.  

**Example:**  
`=GEO_TIMEZONE(51.5, -0.1)`  

---
### `GEO_TIMEZONEOFFSET_STANDARD(lat, lon)`  
Finds the standard timezone offset for a given latitude and longitude.  

- `lat`: The latitude value to lookup.  
- `lon`: The longitude value to lookup.  

**Example:**  
`=GEO_TIMEZONEOFFSET_STANDARD(51.5, -0.1)`  

---  

### `GEO_TIMEZONEOFFSET_DAYLIGHTSAVINGS(lat, lon)`  
Finds the daylight savings timezone offset for a given latitude and longitude.  

- `lat`: The latitude value to lookup.  
- `lon`: The longitude value to lookup.  

**Example:**  
`=GEO_TIMEZONEOFFSET_DAYLIGHTSAVINGS(51.5, -0.1)`  

---
## ⚙️ Sample Workflow

1. **Create a spatial index:**  
   `=GEO_INDEXCREATE("myIndex", A2:A100, B2:B100, C2:C100)`
2. **Perform fast lookups:**  
   `=GEO_INDEXLOOKUPNEAREST("myIndex", 51.5, -0.1, TRUE)`
3. **Clear index when done:**  
   `=GEO_INDEXCLEAR("myIndex")`

---

## ⚠️ Error Handling

Atlas functions follow native Excel error conventions, such as:

- `#N/A` – No match found  
- `#VALUE!` – Invalid input  
- `#NUM!` – Out-of-bounds values

All functions are designed to fail gracefully with clear messages.

---

## 🎯 Why Choose Atlas?

The **Atlas Excel Add-in** gives analysts, engineers, and operations teams the power of spatial computing in the tool they already know: Excel.

- ⚡ Fast, lightweight, and accurate  
- 🌐 Geospatial capabilities with zero external dependencies  
- 🛠️ Optimized for both ad-hoc analysis and production use  
- 🔁 Works seamlessly with large datasets

---

**Atlas Excel Add-in** — bring your location data to life in Excel.  
_Your geospatial toolbox, just a formula away._
