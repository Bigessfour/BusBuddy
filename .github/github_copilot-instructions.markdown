# Copilot Instructions for C# Project

These instructions guide GitHub Copilot Agent when editing or adding features to this C# project. All changes must align with the goal of creating a cutting-edge, professional, and visually appealing user interface (UI) and user experience (UX), as if crafted by an expert UI/UX designer. Adhere to these guidelines for all code modifications, UI enhancements, and new features.

## General Guidelines
- **Scope**: Analyze and modify all relevant files (e.g., C# code, UI components, configuration files) in the workspace using `#workspace` context.
- **Propose Changes**: Before editing, present a plan in the Copilot Edits view, listing files to modify, specific UI improvements, and dependencies, allowing for review and approval.
- **Coding Standards**: Follow C# best practices, use the MVVM pattern for UI separation of concerns, and include comments explaining new or modified UI code.
- **Consistency**: Ensure all changes align with the existing design system and UI patterns in the project.

## UI and UX Standards
All UI components, forms, and features must meet the following requirements to ensure a modern, cohesive, and professional design:

### 1. UI Framework
- Use a modern C# UI framework (e.g., .NET MAUI, Avalonia, Uno Platform for cross-platform, or Fluent UI for Windows) that supports responsive design, animations, and accessibility.
- Add necessary NuGet packages and update `.csproj` files to support the framework.
- Ensure accessibility features (e.g., ARIA attributes, keyboard navigation) are implemented.

### 2. Styling and Theming
- Apply a professional design system (e.g., Material Design, Fluent Design, or a custom theme inspired by 2025 trends like minimalism, neumorphism, or glassmorphism).
- Use modern typography, color schemes, and spacing for a polished look.
- Implement dark and light mode support with seamless theme switching.
- Add subtle animations and transitions for interactive elements (e.g., buttons, menus, modals) to enhance UX without compromising performance.

### 3. Forms
- Design forms to be intuitive, responsive, and visually engaging.
- Use advanced controls (e.g., auto-complete fields, modern date pickers, validated inputs with real-time feedback).
- Optimize layouts for usability with clear labels, proper spacing, and logical flow.
- Ensure forms are accessible and keyboard-navigable.

### 4. UI Components
- Implement sleek, responsive navigation (e.g., sidebars, top bars, hamburger menus).
- Use modern interactive components (e.g., cards, modals, tooltips, progress indicators).
- For data displays, integrate cutting-edge visualization libraries (e.g., LiveCharts2, ScottPlot) for charts and graphs.

### 5. Responsive Design
- Ensure the UI adapts seamlessly to various screen sizes and devices (desktop, tablet, mobile).
- Use flexible layouts (e.g., grid or flexbox equivalents in the chosen framework) for consistency across resolutions.

### 6. Performance
- Optimize UI components for fast rendering and minimal resource usage.
- Avoid performance-heavy features that could degrade the user experience.
- Test changes to ensure no bugs or performance issues are introduced.

### 7. Dependencies and Documentation
- Automatically include required dependencies (e.g., NuGet packages) for new UI features.
- Update project configuration files (e.g., `.csproj`) as needed.
- Document any new setup or usage instructions in `README.md`.

## Adding or Enhancing Features
When implementing new features or enhancing existing ones:
- Ensure new UI components adhere to the above styling, theming, and accessibility standards.
- Prioritize intuitive and visually appealing designs.
- If uncertain about design choices, propose options in the Copilot Edits view, referencing 2025 UI/UX trends.
- Maintain consistency with the project’s existing UI patterns and design system.

## Workflow
1. Analyze the workspace to identify relevant files for the requested change or feature.
2. Propose a detailed plan in the Copilot Edits view, including files to modify, UI improvements, and dependencies.
3. Apply changes iteratively, ensuring alignment with these guidelines.
4. Validate accessibility, responsiveness, and performance of the updated UI.
5. Update `README.md` if new setup or usage instructions are required.

By adhering to these instructions, Copilot ensures all changes contribute to a polished, professional, and state-of-the-art UI for this C# project.