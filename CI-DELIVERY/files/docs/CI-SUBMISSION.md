# CI submission / 自动测试提交说明

## Objective / 目标

Make correctness self-verifying on every push: run real API integration tests
automatically and demonstrate that an incorrect assertion fails CI.

## Implementation / 实现内容

- .NET 10 solution with 7 existing unit tests and 10 new HTTP integration cases.
- WebApplicationFactory runs the actual API endpoints and database migrations.
- Each test uses isolated SQLite in-memory storage, keeping the application database safe.
- Core flow: POST /books (stock 10) → POST /orders (quantity 2) → GET /books/{id} (stock 8).
- Rejected orders verify that stock remains unchanged.
- GitHub Actions runs restore, build and dotnet test on push and pull_request.
- TRX test reports are retained as workflow artifacts.

The API's order endpoint updates inventory and returns an order summary; this
project does not persist a separate order entity. These tests verify its current
HTTP contract and persisted inventory changes.

## Evidence status / 证据状态

Implementation prepared; CI execution and screenshots must be verified before
marking the assignment complete. Do not treat this document as a passing test report.

## Intentional failure / 故意失败验证

1. Start from a passing run on the working branch.
2. In CreateBook_OrderIt_StockDecrementsAndPersists, temporarily change
   `Assert.Equal(8, await GetStock(id));` to
   `Assert.Equal(9, await GetStock(id));`.
3. Commit and push that change. Keep the real dotnet test command unchanged.
4. Confirm the Test step fails because expected stock 9 differs from actual stock 8.
   A restore/build failure is not sufficient evidence.
5. Capture the failing run and its assertion output.
6. Restore the expected stock to 8, commit and push again.
7. Confirm the latest run passes and capture its summary.

## Screenshots / 截图要求

- Green: repository name, workflow run, commit, successful Build and test job,
  and test summary (expected total: 17 passing cases).
- Red: failed Test step, test name, expected 9 versus actual 8, and nonzero exit.
- Optional merge-blocking proof: PR shows merging blocked by a required failed check.
- Save real screenshots as ci-green.png and ci-red.png and include them with submission.
  Screenshots have not been generated or fabricated by this document.

## Merge enforcement / 禁止合并失败代码

Workflow failure and merge protection are separate. In repository settings,
configure a main-branch rule requiring a pull request and the Build and test
check. Restrict bypass as appropriate. Record actual settings or a blocked PR
before claiming that merging is enforced.

## Handoff / 交付

Submit the repository/PR link, README, green and red run links, and screenshots.
Only reassign the original task once its platform, task link and intended
assignee are known and the work is verified.

中文说明：先保存真实的红色失败证据，再把断言恢复为 8，并保存绿色成功证据。
最终代码必须恢复为正确版本。若未配置分支保护，不能写成“系统已禁止合并”。

