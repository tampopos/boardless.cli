const gulp = require('gulp');
const { clean, restore, build, test, publish } = require('gulp-dotnet-cli');
const path = require('path');
const zip = require('gulp-zip');
const spawn = require('child_process').spawn;

const cliName = 'Cli';
const cli = '**/' + cliName + '.csproj';
const appEntries = [cli];
const testEntries = [];
const entries = appEntries.concat(testEntries);
const dist = 'dist';

//clean
gulp.task('clean', () => {
  return gulp.src(entries, { read: false }).pipe(clean());
});
//restore nuget packages
gulp.task('restore', () => {
  return gulp.src(entries, { read: false }).pipe(restore());
});
//compile
gulp.task('build', () => {
  return gulp.src(entries, { read: false }).pipe(build());
});
//run unit tests
gulp.task('test', () => {
  return gulp.src(testEntries, { read: false }).pipe(test());
});
//compile and publish an application to the local filesystem
gulp.task('publish', cb => {
  const output = path.join(process.cwd(), dist, 'raw', 'fdd');
  return gulp
    .src(cli, { read: false })
    .pipe(publish({ configuration: 'Release', output: output }));
});
gulp.task('zip', cb => {
  const raw = dist + '/raw';
  return gulp
    .src(raw + '/**/*', { base: raw })
    .pipe(zip('application.zip'))
    .pipe(gulp.dest(dist));
});
gulp.task('run', () => {
  const cli = spawn('dotnet run', {
    cwd: cliName,
    shell: true
  });
  cli.stdout.on('data', data => console.log('stdout: ' + data));
  cli.stderr.on('data', data => console.log('stdout: ' + data));
  return cli;
});
gulp.task('db', cb => {
  const docker = spawn('bash .docker/database/run.sh', { shell: true });
  docker.stdout.on('data', data => console.log('stdout: ' + data));
  docker.stderr.on('data', data => console.log('stdout: ' + data));
  return docker;
});
gulp.task('migration', () => {
  const migration = spawn('dotnet run migration', {
    cwd: cliName,
    shell: true
  });
  migration.stdout.on('data', data => console.log('stdout: ' + data));
  migration.stderr.on('data', data => console.log('stdout: ' + data));
  return migration;
});
