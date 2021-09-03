if (['dev', 'test', 'prod'].includes(process.env.environment)) {
  console.log("Deployment environment is validated successfully to proceed with workflow");
}

else {
	console.log("Deployment environment must be either 'dev', 'test' or 'prod'");
	process.exit(1);
}