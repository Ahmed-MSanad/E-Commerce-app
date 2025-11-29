Project Overview

This project is a responsive e-commerce product listing application built for the Route Tech Summit Frontend Task. It allows users to browse products, filter and sort them, view detailed product information, and manage a shopping cart. The application integrates with the Fake Store API to fetch product data and simulate cart operations. Key features include:

Product Cards: Displays a list of products with name, image, price, and category. Clicking a product navigates to its details page.

Filtering & Sorting:

Search bar for case-insensitive filtering by product name.

Sort dropdown for sorting by price (low to high, high to low) and name (A-Z).

Product Details Page: Dynamic routing (/products/:id) to display product name, full description, image, price, and category.


Shopping Cart:

Sticky cart sidebar (desktop) or slide-in panel (mobile) to manage selected products.

Add/remove products with quantity tracking.

Save cart and persist locally using localStorage.

Responsive Design: Optimized for mobile and desktop with a fixed cart toggle button and uniform product images.

The application is built with a focus on clean UI, smooth transitions (e.g., cart slide-in/out), and reactive state management using Angular signals.

Screenshot or Demo


Below is a livedemo of the Website:


https://github.com/user-attachments/assets/6e6560fe-d840-4891-8c47-665934756fb3




Tech Stack Used


Angular: Frontend framework for building the SPA, handling routing, components, and reactive state with signals.



Tailwind CSS: Utility-first CSS framework for responsive and customizable styling.



TypeScript: For type-safe development and interfaces (e.g., IProduct, ICartProduct).



Fake Store API: External API for fetching product data and simulating cart operations.



Font Awesome: For icons (e.g., cart, plus, minus).



RxJS: For handling HTTP requests and asynchronous operations.



localStorage: For persisting cart data client-side.

Setup Instructions

Clone the Repository.

Install Dependencies:

npm install

Tailwind
Font Awesome

Run the Application:

ng serve

Open http://localhost:4200 in your browser.

Build for Production:

ng build


Features Implemented


Product Cards: Responsive cards with name, image, price, and category, navigable to details page.

Filtering & Sorting: Search bar and dropdown for filtering by name and sorting by price/name.

Product Details: Dynamic route (/products/:id) showing full product details.



Cart System:



Sticky sidebar (desktop, lg:w-1/6) or slide-in panel (mobile, toggled via fixed cart icon).



Add/remove products with quantity tracking using Angular signals.



Save cart to Fake Store API (POST /carts) and persist in localStorage.



Responsive Design: Mobile-first with Tailwind CSS, including uniform images (w-24 h-24 object-contain).

Optional Enhancements





Shopping Cart: Implemented a fully functional cart with add/remove functionality, quantity tracking, and API integration.



Smooth Transitions: Added 500ms slide-in/out animation for the cart panel on mobile.



Fixed Cart Icon: Prominent button (fixed top-4 right-4) to toggle the cart, always visible.



Error Handling: Displays error messages for API failures (e.g., timeouts).

Notes


The Fake Store API does not persist cart data server-side, so localStorage is used for persistence.



Replace the demo link and screenshot path with actual hosted URLs or repository assets.



Ensure a stable internet connection for API calls to avoid 524 timeout errors.
