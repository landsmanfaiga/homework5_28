import { useRef} from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
    
const Upload = () => {
    const fileRef = useRef();
    const navigate = useNavigate();

    const toBase64 = file => new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });

    const onUploadClick = async () => {
        if (!fileRef.current.files.length) {
            return;
        }

        const base64 = await toBase64(fileRef.current.files[0]);
        await axios.post('/api/person/addpeople', { base64 });
        navigate('/');
    }

    return (
        <div className="container">
            <div className="d-flex vh-100">
                <div className="d-flex w-100 justify-content-center align-self-center">
                    <div className="row">
                        <div className="col-md-10">
                            <input ref={fileRef} className="form-control" type="file" />
                        </div>
                        <div className="col-md-2">
                            <button className="btn btn-primary" onClick={onUploadClick}>Upload</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default Upload;
