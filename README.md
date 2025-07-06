## Usage

-  explain how to use your MVP v2. Provide information for access if needed, for example authentication credentials and so on. Make sure the customer and TA can launch/access and inspect your product after following the instructions in that section.

## Architecture

### Static view

- document the static view on your architecture using UML Component diagram, comment on the coupling and cohesion of your codebase, and discuss how your design decisions affect the maintainability of your product.

### Dynamic view

- document the dynamic view of your architecture using UML Sequence diagram for a non-trivial request that showcases your system. The request must involve several components and multiple transactions between the components.

- Test and report how much time this scenario takes to execute in your production environment.

### Deployment view

- document the deployment view of your architecture (can be a custom view with a legend), comment on the deployment choices and how it is to be deployed on the customer’s side.

## Development

### Kanban board

- [Link to kanban board](https://some-link)

- Document entry criteria for each column and update board according with what you described

### Git workflow

- Specify which base workflow you adapted to your context (e.g., GitHub flow, Gitflow, etc).

- Explain / Define rules for:
  1. Creating issues from the defined templates (link the earlier defined templates);
  2. Labelling issues;
  3. Assigning issues to team members;
  4. Creating, naming, merging branches;
  5. Commit messages format;
  6. Creating a pull request for an issue using a pull request template (link the earlier defined pull request template);
  7. Code reviews;
  8. Merging pull requests;
  9. Resolving issues.

- Illustrate your git workflow using a Gitgraph diagram.

### Secrets management

- Document your rules for secrets management (e.g. passwords or API keys). Mention where you store your secrets without revealing sensitive information.

## Quality assurance

ЭТО НЕ НАДО ДЕЛАТЬ В РИДМИ, НО НАДО СДЕЛАТЬ В ДРУГОМ МЕСТЕ: Read through ISO model 25010 with the whole team
Pick at least 3 quality sub-characteristics important to your customer and confirm them with the customer.
In the “docs/quality-assurance/quality-attribute-scenarios.md” file, group chosen sub-characteristics
in “## <characteristic name>” sections and for each chosen sub-characteristic, in a subsubsection “### <sub-characteristic name>”
For each:
  1. Explain why that sub-characteristic is important to your customer.
  2. Provide tests for that sub-characteristic in Quality Attribute Scenario format, each in its own subsubsubsection with a unique title.
  3. For each test, specify how exactly you are going to execute it.

### Quality attribute scenarios

- provide a link to the “docs/quality-assurance/quality-attribute-scenarios.md” file.

### Automated tests

- Which tools you used for testing.
- Which types of tests you implemented.
- Where tests of each type are in the repository.

## Build and deployment

### Continuous Integration

- For each CI workflow file:
  1. Provide a link to that file.
  2. Provide a list of static analysis tools and testing tools that you use in the CI part. For each tool in the list, briefly explain what you use it for.

- Provide a link to where all CI workflow runs can be seen.
