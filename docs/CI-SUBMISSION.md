# Integration Testing and CI Submission Report

Repository: [zyanonymoust/library](https://github.com/zyanonymoust/library)

Branch: `codex/integration-tests-ci`

## Objective

Run correctness checks on every push and pull request, exercise a realistic book creation and ordering flow through the API, and prove CI fails when an assertion is broken.

## Implementation

- Added an ASP.NET Core integration test project using `WebApplicationFactory<Program>`.
- The factory starts the actual API pipeline and uses isolated SQLite in-memory storage. The tests exercise the real controllers, service, repository, and Entity Framework migrations without relying on the checked-in database file.
- The integration suite has 10 cases: book creation and persisted stock decrement; insufficient stock; zero and negative order quantities; ordering all stock then trying again; missing books; invalid book IDs; empty title; and delete followed by lookup/order.
- The existing test project contributes 7 unit tests.
- `.github/workflows/dotnet-tests.yml` runs restore, Release build, and `dotnet test` on every push and pull request. It uploads TRX output even after a failure. Test failures propagate a non-zero exit status.

## Verified results

| Run | Result | Evidence |
| --- | --- | --- |
| #2 — Fix API integration test response handling | Passed: 7 unit tests + 10 integration tests | [GitHub Actions run](https://github.com/zyanonymoust/library/actions/runs/38190513043) |
| #3 — Test CI detect a broken assertion | Expected failure: Expected 9, Actual 8; Test step exited with code 1 | [GitHub Actions run](https://github.com/zyanonymoust/library/actions/runs/38191323555) |
| #4 — Re-back | Passed after restoring the assertion: 17 total tests | [GitHub Actions run](https://github.com/zyanonymoust/library/actions/runs/38191702518) |

Run #1 failed because the initial test did not read the book ID correctly. That test issue was corrected before Run #2 and before the deliberate failure verification.

## Screenshot evidence

Save the supplied GitHub Actions screenshots alongside this report as `docs/evidence/ci-red.png` and `docs/evidence/ci-green.png` if image files are available. The red screenshot should show the failed Test step and `Expected: 9` / `Actual: 8`. The green screenshot should show Run #4 and the 17 passing tests. The run links above provide online evidence if local screenshot files are not included.

## Merge protection

The workflow produces a failing status check on test failure. Whether GitHub blocks merging is controlled separately by repository rules. Configure the `Build and test` check as required on the target branch if merge blocking is required. This report does not claim branch protection has been configured or verified.

## Completion checklist

- [x] HTTP integration tests exercise create book → order → persisted stock decrement.
- [x] GitHub Actions runs on push and pull request.
- [x] CI failure on a deliberately wrong test assertion verified.
- [x] Assertion restored and final recorded run passed.
- [ ] Attach local screenshot files if the assignment requires image attachments in addition to run links.
- [ ] Configure required status checks if merge blocking is required.
