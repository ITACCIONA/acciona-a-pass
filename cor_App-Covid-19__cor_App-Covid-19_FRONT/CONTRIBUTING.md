# Contributing to A-Pass 

As a contributor, here are the guidelines we would like you to follow:

## Submission Guidelines

### Submitting a Pull Request (PR)

Before you submit your Pull Request (PR) consider the following guidelines:

1. Make your changes in a new git branch:

   ```shell
   git checkout -b my-fix-branch develop
   ```

2. Follow our Coding Rules.
3. Run the full test suite, and ensure that all tests pass.
4. Commit your changes using a descriptive commit message that follows ourcommit message conventions. Adherence to these conventions
   is necessary because release notes are automatically generated from these messages.

5. Push your branch to Bitbucket:

   ```shell
   git push origin my-fix-branch
   ```

6. Make sure your branch is up-to-date with `develop`, merge the branch develop into your branch.

7. In Bitbucket, send a pull request to `develop`.

- If the reviewer suggest changes then:

  - Make the required updates.
  - Re-run the test suites to ensure tests are still passing.
  - Push yout changes to the repository.

That's it! Thank you for your contribution!

#### After your pull request is merged

After your pull request is merged, you can safely delete your branch and pull the changes from the main repository:

- Check out the master branch:

  ```shell
  git checkout master -f
  ```

- Delete the local branch:

  ```shell
  git branch -D my-fix-branch
  ```

## Coding Rules

To ensure consistency throughout the source code, keep these rules in mind as you are working:

- All features or bug fixes **must be tested** by one or more specs (unit-tests).

- We wrap all code at **120 characters**. An automated formatter (prettier) is available and setted for format on save.

## Commit Message Guidelines

We have very precise rules over how our git commit messages can be formatted. This leads to **more
readable messages** that are easy to follow when looking through the **project history**. But also,
we use the git commit messages to **generate the change log with lerna**.

### Commit Message Format

We use [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) and to be sure you respect the correct format, we have a Husky hook that checks it.

Each commit message consists of a **header**, a **body** and a **footer**. The header has a special
format that includes a **type**, a **scope** and a **subject**:

```
<type>(<scope>): <subject>
<BLANK LINE>
<body>
<BLANK LINE>
<footer>
```

The **header** is mandatory and the **scope** of the header is optional.

Any line of the commit message cannot be longer 100 characters! This allows the message to be easier
to read in various git tools.

The footer should contain a closing reference to an issue if any.

Samples:

```
docs(changelog): update changelog to beta.5
```

```
fix(release): need to depend on latest rxjs and zone.js

The version in our package.json gets copied to the one we publish, and users need the latest of these.
```

### Revert

If the commit reverts a previous commit, it should begin with `revert:`, followed by the header of the reverted commit. In the body it should say: `This reverts commit <hash>.`, where the hash is the SHA of the commit being reverted.

### Type

Must be one of the following:

- **build**: Changes that affect the build system or external dependencies (example scopes: gulp, broccoli, npm)
- **ci**: Changes to our CI configuration files and scripts (example scopes: Travis, Circle, BrowserStack, SauceLabs)
- **docs**: Documentation only changes
- **feat**: A new feature
- **fix**: A bug fix
- **perf**: A code change that improves performance
- **refactor**: A code change that neither fixes a bug nor adds a feature
- **style**: Changes that do not affect the meaning of the code (white-space, formatting, missing semi-colons, etc)
- **test**: Adding missing tests or correcting existing tests

### Scope

We don't have right now a special convention for the scope, but use the package name is a good practice.

### Subject

The subject contains a succinct description of the change:

- use the imperative, present tense: "change" not "changed" nor "changes"
- don't capitalize the first letter
- no dot (.) at the end

### Body

Just as in the **subject**, use the imperative, present tense: "change" not "changed" nor "changes".
The body should include the motivation for the change and contrast this with previous behavior.

### Footer

The footer should contain any information about **Breaking Changes** and is also the place to
reference GitHub issues that this commit **Closes**.

**Breaking Changes** should start with the word `BREAKING CHANGE:` with a space or two newlines. The rest of the commit message is then used for this.

## Recomended VSCode Pluggins / tools

- Angular Essentials
- GitLens
- Jest
- Jest Snippets
- TODO Hightlight
