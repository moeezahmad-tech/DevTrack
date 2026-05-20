# DevTrack

DevTrack is an application built in C# for tracking departments, employee roles, project lifecycles, and tasks. Built on top of a highly optimized, normalized relational schema with full referential integrity.

---

## 👥 Group Members

* **Moeez Ahmad** (Reg No: 2025-GA-89)
* **Ali Asjad** (Reg No: 2025-GA-68)
* **Hamza Hameed** (Reg No: 2025-GA-103)
* **M. Abdullah** (Reg No: 2025-GA-63)

---

## 🛠️ Tech Stack & Core Concepts

* **Language:** C#
* **Database:** MySQL
* **Architecture Rules:** 3NF (Third Normal Form) database design, Object-Oriented Programming (OOP) mappings, and strict referential integrity constraints.

---

## 🗄️ Database Architecture (3NF)

The system maps out organization workflows by handling core entities and specialized user inheritance paths cleanly:

* **Core Entities:** `Person`, `Department`, `Project`, `Task`, `Reports`
* **Inheritance Rules:** Specialized roles (`Employee` and `Head of Dept`) inherit primary identifiers directly from the base `Person` record.

### Schema Blueprint
1. **Person:** Base entity tracking unique identities, credentials, and basic metrics.
2. **Department:** Segmented organizational hubs managing dedicated teams.
3. **Employee / Head of Dept:** Role-specific mappings linked dynamically back to department records.
4. **Project:** Lifecycles tracked directly under supervising departments.
5. **Task:** Action items tracking deadlines, execution priorities, assigned developers, and parent project boundaries.
6. **Reports:** Status validation and automated deployment feedback updates.

---

## 🚀 Getting Started

1. Clone the repository:
   ```bash
   git clone [https://github.com/moeezahmad-tech/DevTrack](https://github.com/moeezahmad-tech/DevTrack)