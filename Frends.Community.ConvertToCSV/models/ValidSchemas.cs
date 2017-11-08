namespace FRENDS
{
    public static class ValidSchemas
    {
        private static string _validJsonSchema;

        public static string ValidJsonSchema
        {
            get
            {
                if(string.IsNullOrEmpty(_validJsonSchema)) {
                    _validJsonSchema = @"
                    {
	                    '$schema': 'http://json-schema.org/schema#',
    
                        'type': 'object',
	                    'patternProperties': {
    	                    '.*':{
       		                    'type':'array',
     		                    'items': {
             	                    'type':'object',
                                    'patternProperties': {
                 	                    '.*':{
                                          'type':'string'
                                        } 
                                    }
                                }
     	                    } 
                        }  
                    }
                ";
                }
                return _validJsonSchema;
            }
        }
    }
}
